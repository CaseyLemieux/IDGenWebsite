using IDGenWebsite.Data;
using IDGenWebsite.Models;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using RestSharp;
using RestSharp.Authenticators;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace IDGenWebsite.Utilities
{
    public class ApiHelper
    {
        private readonly SchoolContext _schoolContext;
        private string baseUrl;
        private string clientId;
        private string clientSecret;
        private RestClient restClient;

        public ApiHelper(SchoolContext schoolContext)
        {
            _schoolContext = schoolContext;
            baseUrl = "https://franklin.focusschoolsoftware.com/focus/api/1.0/ims/oneroster/v1p1";
            clientId = "4173e207-702a-4300-aaf2-30c8aa9427c2";
            clientSecret = "f4e1a06d-cf5d-40da-b62a-0d2eae010ac2";
            restClient = new RestClient(baseUrl);
            RestRequest request = new RestRequest("token") { Method = Method.POST };
            request.AddHeader("Accept", "application/json");
            request.AddHeader("Content-Type", "application/x-www-form-urlencoded");
            request.AddParameter("client_id", clientId);
            request.AddParameter("client_secret", clientSecret);
            request.AddParameter("grant_type", "client_credentials");
            var tresponse = restClient.Execute(request);
            var responseJson = tresponse.Content;
            var token = JsonConvert.DeserializeObject<Dictionary<string, object>>(responseJson)["access_token"].ToString();
            Console.WriteLine("Import Started requesting token");
            //Add the token to the header for the rest of the api requests
            restClient.Authenticator = new JwtAuthenticator(token);
        }

        public async Task GetOrgs()
        {
            //Make request to Focus API for the orgs
            RestRequest orgRequest = new RestRequest("/orgs?limit=500&offset=0");
            var orgResponse = await restClient.ExecuteAsync(orgRequest);
            //Get the full JSON content from the response
            var orgJson = orgResponse.Content;
            //Create a jobject
            var payload = JObject.Parse(orgJson);
            //Get the orgs array and split them into arrays to be deserialized into objects. 
            var array = JArray.Parse(payload["orgs"].ToString());
            //List<Organizations> orgs = new List<Organizations>();
            for (int i = 0; i < array.Count; i++)
            {
                Organizations newOrg = JsonConvert.DeserializeObject<Organizations>(array[i].ToString());
                await _schoolContext.Orgs.AddAsync(newOrg);
            }
            await _schoolContext.SaveChangesAsync();
        }

        public async Task GetAcademicSessions()
        {
            //Make request to Focus API for the Academic Sessions
            RestRequest sessionsRequest = new RestRequest("/academicSessions?limit=500&offset=0");
            var sessionsResponse = await restClient.ExecuteAsync(sessionsRequest);
            //Get the full JSON content from the response
            var sessionsJson = sessionsResponse.Content;
            //Create a jobject
            var sessionsPayload = JObject.Parse(sessionsJson);
            //Get the orgs array and split them into arrays to be deserialized into objects. 
            var sessionsArray = JArray.Parse(sessionsPayload["academicSessions"].ToString());
            List<AcademicSessions> academicSessions = new List<AcademicSessions>();
            for (int i = 0; i < sessionsArray.Count; i++)
            {
                AcademicSessions session = JsonConvert.DeserializeObject<AcademicSessions>(sessionsArray[i].ToString());
                academicSessions.Add(session);
            }

            //Will have duplicate refrences for parent objects that need to be resolved. 
            foreach (var term in academicSessions)
            {
                if (term.Parent == null)
                {
                   await _schoolContext.AddAsync(term);
                   await _schoolContext.SaveChangesAsync();
                }
                else
                {
                    term.Parent_SourcedId = term.Parent.Parent_SourcedId;
                    term.Parent = null;
                    await _schoolContext.AddAsync(term);
                    await _schoolContext.SaveChangesAsync();
                }
            }
        }

        public async Task GetUsers()
        {
            //Make request to the Focus API for the Users in Student and Teacher roles
            RestRequest usersRequest = new RestRequest("/users?limit=500&offset=0&filter=role='student' OR role='teacher'");
            //List<Users> users = new List<Users>();
            string next = null;
            do
            {

                var usersResponse = await restClient.ExecuteAsync(usersRequest);
                //Get the full JSON content from the response
                var usersJson = usersResponse.Content;
                //Create a jObject
                var usersPayload = JObject.Parse(usersJson);
                //Get the users and split them into 
                var usersArray = JArray.Parse(usersPayload["users"].ToString());
                //Get Total
                //var totalUsers = usersPayload["total"].ToString();

                for (int i = 0; i < usersArray.Count; i++)
                {
                    //var temp = usersArray[i].ToString();
                    //Convert User JSON to a User Object
                    Users user = JsonConvert.DeserializeObject<Users>(usersArray[i].ToString());

                    //Get the User Metadata fields and convert them to a Metadata Object
                    //Then add those Objects to the User Metadata List
                    var values = user.TempMetadata.ToObject<Dictionary<string, string>>();
                    List<Metadatas> metadatas = new List<Metadatas>();
                    foreach (var value in values)
                    {
                        if (value.Value != null)
                        {
                            Metadatas metadata = new Metadatas();
                            metadata.Key = value.Key;
                            metadata.Value = value.Value;
                            metadatas.Add(metadata);
                        }
                    }
                    user.Metadata = metadatas;

                    //Convert the String List of Grades into Grade Objects and add those to the user
                    List<Grades> gradesList = new List<Grades>();
                    foreach (var grade in user.StringGrades)
                    {
                        Grades grades = new Grades();
                        if (grades.CedsLevels.ContainsKey(grade))
                        {
                            grades.Code = grade;
                            grades.Description = grades.CedsLevels[grade];
                            gradesList.Add(grades);
                        }

                    }
                    user.Grades = gradesList;

                    //The User Orgs will need to be replaced with the actual org exsisting in the database.
                    List<Organizations> orgsInDb = new List<Organizations>();
                    foreach (var org in user.Organizations)
                    {

                        var orgInDb = _schoolContext.Orgs.Where(o => o.OrgSourcedId == org.OrgSourcedId).FirstOrDefault();
                        orgsInDb.Add(orgInDb);

                    }
                    user.Organizations = orgsInDb;
                    //Add the user to the users array
                    //users.Add(user);

                    //Add the user to the database and save them
                    await _schoolContext.Users.AddAsync(user);

                }
                next = usersPayload["pagination"]["next"].ToString();
                usersRequest = new RestRequest(next);
            } while (next != "");
            try
            {

                await _schoolContext.SaveChangesAsync();
            }
            catch (DbUpdateException ex)
            {
                Console.WriteLine(ex.InnerException);
                _schoolContext.ChangeTracker.Clear();
            }
        }

        public async Task GetCourses()
        {
            //Make request to the Focus API for the Courses
            RestRequest coursesRequest = new RestRequest("/courses?limit=500&offset=0");
            //List<Courses> courses = new List<Courses>();
            string next = null;
            do
            {
                var coursesResponse = await restClient.ExecuteAsync(coursesRequest);
                //Get the full JSON content from the response
                var coursesJson = coursesResponse.Content;
                //Create a jObject
                var coursesPayload = JObject.Parse(coursesJson);
                //Get the courses and split them into an array
                var coursesArray = JArray.Parse(coursesPayload["courses"].ToString());


                for (int i = 0; i < coursesArray.Count; i++)
                {
                    //Convert Course JSON to a Course Object
                    Courses course = JsonConvert.DeserializeObject<Courses>(coursesArray[i].ToString());

                    //Get the course Metadata fields and convert them to a Metadata Object
                    //Then add those Objects to the User Metadata List
                    if (course.TempMetadata != null)
                    {
                        var values = course.TempMetadata.ToObject<Dictionary<string, string>>();
                        List<Metadatas> metadatas = new List<Metadatas>();
                        foreach (var value in values)
                        {
                            if (value.Value != null)
                            {
                                Metadatas metadata = new Metadatas();
                                metadata.Key = value.Key;
                                metadata.Value = value.Value;
                                metadatas.Add(metadata);
                            }
                        }
                        course.Metadata = metadatas;
                    }

                    if (course.StringGrades.Count > 0)
                    {
                        //Convert the String List of Grades into Grade Objects and add those to the course
                        List<Grades> gradesList = new List<Grades>();
                        foreach (var grade in course.StringGrades)
                        {
                            Grades grades = new Grades();
                            if (grades.CedsLevels.ContainsKey(grade))
                            {
                                grades.Code = grade;
                                grades.Description = grades.CedsLevels[grade];
                                gradesList.Add(grades);
                            }

                        }
                        course.Grades = gradesList;
                    }

                    //For now just import the subject. Will look into importing subject codes at a later date. 
                    if (course.StringSubjects.Count > 0)
                    {
                        List<Subjects> subjectsList = new List<Subjects>();
                        foreach (var subject in course.StringSubjects)
                        {
                            Subjects subjects = new Subjects
                            {
                                Title = subject
                            };
                            subjectsList.Add(subjects);
                        }
                        course.Subjects = subjectsList;
                    }

                    //The academic session will need to be replaced with the actual session exsisting in the database. 
                    course.SessionSourcedId = course.SchoolYear.SessionSourcedId;
                    course.SchoolYear = null;

                    //The course Orgs will need to be replaced with the actual org exsisting in the database.
                    course.OrgSourcedId = course.Organization.OrgSourcedId;
                    course.Organization = null;

                    //Add the course to the database and save them
                    await _schoolContext.Courses.AddAsync(course);

                }
                next = coursesPayload["pagination"]["next"].ToString();
                coursesRequest = new RestRequest(next);
            } while (next != "");
            try
            {

                await _schoolContext.SaveChangesAsync();
            }
            catch (DbUpdateException ex)
            {
                Console.WriteLine(ex.InnerException);
                _schoolContext.ChangeTracker.Clear();
            }
        }

        public async Task GetClasses()
        {
            //Make request to the Focus API for the Classes
            RestRequest classesRequest = new RestRequest("/classes?limit=500&offset=0");
            //List<Classes> classes = new List<Classes>();
            string next = null;
            do
            {
                var classesResponse = await restClient.ExecuteAsync(classesRequest);
                //Get the full JSON content from the response
                var classesJson = classesResponse.Content;
                //Create a jObject
                var classesPayload = JObject.Parse(classesJson);
                //Get the Classes and split them into an array
                var classesArray = JArray.Parse(classesPayload["classes"].ToString());


                for (int i = 0; i < classesArray.Count; i++)
                {
                    //Convert Class JSON to a Class Object
                    Classes classObject = JsonConvert.DeserializeObject<Classes>(classesArray[i].ToString());

                    //Get the Class Metadata fields and convert them to a Metadata Object
                    //Then add those Objects to the Class Metadata List
                    if (classObject.TempMetadata != null)
                    {
                        var values = classObject.TempMetadata.ToObject<Dictionary<string, string>>();
                        List<Metadatas> metadatas = new List<Metadatas>();
                        foreach (var value in values)
                        {
                            if (value.Value != null)
                            {
                                Metadatas metadata = new Metadatas();
                                metadata.Key = value.Key;
                                metadata.Value = value.Value;
                                metadatas.Add(metadata);
                            }
                        }
                        classObject.Metadata = metadatas;
                    }

                    if (classObject.StringGrades.Count > 0)
                    {
                        //Convert the String List of Grades into Grade Objects and add those to the class
                        List<Grades> gradesList = new List<Grades>();
                        foreach (var grade in classObject.StringGrades)
                        {
                            Grades grades = new Grades();
                            if (grades.CedsLevels.ContainsKey(grade))
                            {
                                grades.Code = grade;
                                grades.Description = grades.CedsLevels[grade];
                                gradesList.Add(grades);
                            }

                        }
                        classObject.Grades = gradesList;
                    }

                    //For now just import the subject. Will look into importing subject codes at a later date. 
                    if (classObject.StringSubjects.Count > 0)
                    {
                        List<Subjects> subjectsList = new List<Subjects>();
                        foreach (var subject in classObject.StringSubjects)
                        {
                            Subjects subjects = new Subjects
                            {
                                Title = subject
                            };
                            subjectsList.Add(subjects);
                        }
                        classObject.Subjects = subjectsList;
                    }

                    //The academic sessions will need to be replaced with the actual sessions exsisting in the database. 
                    List<AcademicSessions> sessionsInDb = new List<AcademicSessions>();
                    foreach (var session in classObject.AcademicSessions)
                    {

                        var sessionInDb = _schoolContext.AcademicSessions.Where(s => s.SessionSourcedId == session.SessionSourcedId).FirstOrDefault();
                        sessionsInDb.Add(sessionInDb);

                    }
                    classObject.AcademicSessions = sessionsInDb;

                    //The Class Orgs will need to be replaced with the actual org exsisting in the database.
                    classObject.OrgSourcedId = classObject.School.OrgSourcedId;
                    classObject.School = null;

                    //The Class Course will need to be replaced with the actual Course in the database. 
                    classObject.CourseSourcedId = classObject.Course.CourseSourcedId;
                    classObject.Course = null;

                    //Add the Class to the database and save them
                    await _schoolContext.Classes.AddAsync(classObject);



                }
                next = classesPayload["pagination"]["next"].ToString();
                classesRequest = new RestRequest(next);
            } while (next != "");
            try
            {

                await _schoolContext.SaveChangesAsync();
            }
            catch (DbUpdateException ex)
            {
                Console.WriteLine(ex.InnerException);
                _schoolContext.ChangeTracker.Clear();
            }
        }

        public async Task GetEnrollments()
        {
            //Make request to the Focus API for the Enrollments 
            RestRequest enrollmentsRequest = new RestRequest("/enrollments?limit=500&offset=0");
            List<Enrollments> enrollments = new List<Enrollments>();
            string next = null;

            do
            {
                var enrollmentsResponse = await restClient.ExecuteAsync(enrollmentsRequest);
                //Get the full JSON content from the response
                var enrollmentsJson = enrollmentsResponse.Content;
                //Create a jObject
                var enrollmentsPayload = JObject.Parse(enrollmentsJson);
                //Get the Enrollments and split them into an array
                var enrollmentsArray = JArray.Parse(enrollmentsPayload["enrollments"].ToString());

                for (int i = 0; i < enrollmentsArray.Count; i++)
                {
                    //Convert Enrollment JSON to a Enrollment Object
                    Enrollments enrollment = JsonConvert.DeserializeObject<Enrollments>(enrollmentsArray[i].ToString());

                    if (enrollment.Class != null && enrollment.School != null && enrollment.User != null)
                    {
                        //Get the Enrollment Metadata fields and convert them to a Metadata Object
                        //Then add those Objects to the Enrollment Metadata List
                        if (enrollment.TempMetadata != null)
                        {
                            var values = enrollment.TempMetadata.ToObject<Dictionary<string, string>>();
                            List<Metadatas> metadatas = new List<Metadatas>();
                            foreach (var value in values)
                            {
                                if (value.Value != null)
                                {
                                    Metadatas metadata = new Metadatas();
                                    metadata.Key = value.Key;
                                    metadata.Value = value.Value;
                                    metadatas.Add(metadata);
                                }
                            }
                            enrollment.Metadata = metadatas;
                        }

                        //Set the FK for User
                        enrollment.UserSourcedId = enrollment.User.UserSourcedId;
                        enrollment.User = null;


                        //set the FK of the Org
                        enrollment.OrgSourcedId = enrollment.School.OrgSourcedId;
                        enrollment.School = null;

                        //Set the fk of the class 
                        enrollment.ClassSourcedId = enrollment.Class.ClassSourcedId;
                        enrollment.Class = null;
                        await _schoolContext.Enrollments.AddAsync(enrollment);

                        //enrollments.Add(enrollment);
                    }
                    //Add the enrollment to the database and save them

                }

                next = enrollmentsPayload["pagination"]["next"].ToString();
                enrollmentsRequest = new RestRequest(next);
            } while (next != "");

            try
            {

                await _schoolContext.SaveChangesAsync();
            }
            catch (DbUpdateException ex)
            {
                Console.WriteLine(ex.InnerException);
                _schoolContext.ChangeTracker.Clear();
            }
        }
    }
}
