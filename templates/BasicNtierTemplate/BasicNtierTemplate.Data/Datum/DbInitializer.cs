using BasicNtierTemplate.Data.Enums;
using BasicNtierTemplate.Data.Model;

namespace BasicNtierTemplate.Data.Datum
{
    public static class DbInitializer
    {
        public static void Initialize(BasicNtierTemplateDbContext dbContext)
        {
            dbContext.Database.EnsureCreated();

            // Look for any students.
            if (dbContext.Students.Any())
            {
                return;   // DB has been seeded
            }

            var students = GetStudents();
            dbContext.Students.AddRange(students);
            dbContext.SaveChanges();

            var courses = GetCourses();
            dbContext.Courses.AddRange(courses);
            dbContext.SaveChanges();

            var enrollments = GetEnrollments();
            dbContext.Enrollments.AddRange(enrollments);
            dbContext.SaveChanges();
        }

        private static IEnumerable<Enrollment> GetEnrollments()
        {
            return
                [
                    new Enrollment{ CourseId = 1050, StudentId = 1, Grade = Grade.A },
                    new Enrollment{ CourseId = 4022, StudentId = 1, Grade = Grade.C },
                    new Enrollment{ CourseId = 4041, StudentId = 1, Grade = Grade.B },
                    new Enrollment{ CourseId = 1045, StudentId = 2, Grade = Grade.B },
                    new Enrollment{ CourseId = 3141, StudentId = 2, Grade = Grade.F },
                    new Enrollment{ CourseId = 2021, StudentId = 2, Grade = Grade.F },
                    new Enrollment{ CourseId = 1050, StudentId = 3 },
                    new Enrollment{ CourseId = 1050, StudentId = 4 },
                    new Enrollment{ CourseId = 4022, StudentId = 4, Grade = Grade.F },
                    new Enrollment{ CourseId = 4041, StudentId = 5, Grade = Grade.C },
                    new Enrollment{ CourseId = 1045, StudentId = 6 },
                    new Enrollment{ CourseId = 3141, StudentId = 7, Grade = Grade.A }
                ];
        }

        private static IEnumerable<Course> GetCourses()
        {
            return
                [
                    new Course { Id = 1050, Title = "Chemistry", Credits = 3 },
                    new Course { Id = 4022, Title = "Microeconomics", Credits = 3 },
                    new Course { Id = 4041, Title = "Macroeconomics", Credits = 3 },
                    new Course { Id = 1045, Title = "Calculus", Credits = 4 },
                    new Course { Id = 3141, Title = "Trigonometry", Credits = 4 },
                    new Course { Id = 2021, Title = "Composition", Credits = 3 },
                    new Course { Id = 2042, Title = "Literature", Credits = 4 }
               ];
        }

        private static IEnumerable<Student> GetStudents()
        {
            return
                [
                    new Student{ GovernmentId = "45892341", FirstMidName = "Carson", LastName = "Alexander", EnrollmentDate = DateTime.Parse("1999-09-01 10:23:45") },
                    new Student{ GovernmentId = "52173456", FirstMidName = "Meredith", LastName = "Alonso", EnrollmentDate = DateTime.Parse("2021-09-01 11:15:32") },
                    new Student{ GovernmentId = "48765234", FirstMidName = "Arturo", LastName = "Anand", EnrollmentDate = DateTime.Parse("2003-09-01 12:47:18") },
                    new Student{ GovernmentId = "41234567", FirstMidName = "Gytis", LastName = "Barzdukas", EnrollmentDate = DateTime.Parse("1999-09-01 10:55:23") },
                    new Student{ GovernmentId = "59876543", FirstMidName = "Yan", LastName = "Li", EnrollmentDate = DateTime.Parse("2002-09-01 11:32:07") },
                    new Student{ GovernmentId = "43567890", FirstMidName = "Peggy", LastName = "Justice", EnrollmentDate = DateTime.Parse("2001-09-01 10:18:54") },
                    new Student{ GovernmentId = "56789012", FirstMidName = "Laura", LastName = "Norman", EnrollmentDate = DateTime.Parse("2003-09-01 12:25:41") },
                    new Student{ GovernmentId = "47890123", FirstMidName = "Nino", LastName = "Olivetto", EnrollmentDate = DateTime.Parse("2010-09-01 11:08:19") },
                    new Student{ GovernmentId = "51234568", FirstMidName = "Brandon", LastName = "Mitchell", EnrollmentDate = DateTime.Parse("1995-03-15 10:42:33") },
                    new Student{ GovernmentId = "44567891", FirstMidName = "Emily", LastName = "Johnson", EnrollmentDate = DateTime.Parse("2018-01-22 11:55:12") },
                    new Student{ GovernmentId = "58901234", FirstMidName = "Michael", LastName = "Davis", EnrollmentDate = DateTime.Parse("2007-06-10 12:14:28") },
                    new Student{ GovernmentId = "42345679", FirstMidName = "Sarah", LastName = "Wilson", EnrollmentDate = DateTime.Parse("2012-11-03 10:37:56") },
                    new Student{ GovernmentId = "55678902", FirstMidName = "Joshua", LastName = "Martinez", EnrollmentDate = DateTime.Parse("1998-08-17 11:21:44") },
                    new Student{ GovernmentId = "49012345", FirstMidName = "Ashley", LastName = "Anderson", EnrollmentDate = DateTime.Parse("2015-04-29 12:03:15") },
                    new Student{ GovernmentId = "53456780", FirstMidName = "Christopher", LastName = "Taylor", EnrollmentDate = DateTime.Parse("2001-12-08 10:49:37") },
                    new Student{ GovernmentId = "46789013", FirstMidName = "Jessica", LastName = "Thomas", EnrollmentDate = DateTime.Parse("2019-07-14 11:26:09") },
                    new Student{ GovernmentId = "40123456", FirstMidName = "Matthew", LastName = "Jackson", EnrollmentDate = DateTime.Parse("2005-02-21 12:38:52") },
                    new Student{ GovernmentId = "54567892", FirstMidName = "Amanda", LastName = "White", EnrollmentDate = DateTime.Parse("2013-10-06 10:11:25") },
                    new Student{ GovernmentId = "47890124", FirstMidName = "Daniel", LastName = "Harris", EnrollmentDate = DateTime.Parse("1997-05-19 11:44:18") },
                    new Student{ GovernmentId = "51234569", FirstMidName = "Stephanie", LastName = "Martin", EnrollmentDate = DateTime.Parse("2020-09-27 12:22:46") },
                    new Student{ GovernmentId = "45678903", FirstMidName = "David", LastName = "Thompson", EnrollmentDate = DateTime.Parse("2008-03-12 10:58:31") },
                    new Student{ GovernmentId = "59012346", FirstMidName = "Jennifer", LastName = "Garcia", EnrollmentDate = DateTime.Parse("2016-11-25 11:17:04") },
                    new Student{ GovernmentId = "43456781", FirstMidName = "Andrew", LastName = "Martinez", EnrollmentDate = DateTime.Parse("2002-08-03 12:41:59") },
                    new Student{ GovernmentId = "56789014", FirstMidName = "Nicole", LastName = "Robinson", EnrollmentDate = DateTime.Parse("2011-01-16 10:28:14") },
                    new Student{ GovernmentId = "50123457", FirstMidName = "Ryan", LastName = "Clark", EnrollmentDate = DateTime.Parse("1999-06-30 11:53:27") },
                    new Student{ GovernmentId = "44567893", FirstMidName = "Samantha", LastName = "Rodriguez", EnrollmentDate = DateTime.Parse("2017-04-08 12:09:43") },
                    new Student{ GovernmentId = "58901235", FirstMidName = "Justin", LastName = "Lewis", EnrollmentDate = DateTime.Parse("2004-12-19 10:35:58") },
                    new Student{ GovernmentId = "42345680", FirstMidName = "Elizabeth", LastName = "Lee", EnrollmentDate = DateTime.Parse("2014-07-02 11:48:21") },
                    new Student{ GovernmentId = "55678904", FirstMidName = "Kevin", LastName = "Walker", EnrollmentDate = DateTime.Parse("2000-02-14 12:16:37") },
                    new Student{ GovernmentId = "49012347", FirstMidName = "Heather", LastName = "Hall", EnrollmentDate = DateTime.Parse("2009-10-28 10:44:52") },
                    new Student{ GovernmentId = "53456782", FirstMidName = "Brian", LastName = "Allen", EnrollmentDate = DateTime.Parse("2022-05-11 11:29:06") },
                    new Student{ GovernmentId = "46789015", FirstMidName = "Michelle", LastName = "Young", EnrollmentDate = DateTime.Parse("1996-09-23 12:52:19") },
                    new Student{ GovernmentId = "40123458", FirstMidName = "Jason", LastName = "Hernandez", EnrollmentDate = DateTime.Parse("2006-03-07 10:07:34") },
                    new Student{ GovernmentId = "54567894", FirstMidName = "Melissa", LastName = "King", EnrollmentDate = DateTime.Parse("2015-11-20 11:39:48") },
                    new Student{ GovernmentId = "47890126", FirstMidName = "James", LastName = "Wright", EnrollmentDate = DateTime.Parse("2001-08-04 12:24:11") },
                    new Student{ GovernmentId = "51234570", FirstMidName = "Rebecca", LastName = "Lopez", EnrollmentDate = DateTime.Parse("2019-01-17 10:51:26") },
                    new Student{ GovernmentId = "45678905", FirstMidName = "Tyler", LastName = "Hill", EnrollmentDate = DateTime.Parse("2010-06-29 11:13:42") },
                    new Student{ GovernmentId = "59012348", FirstMidName = "Amy", LastName = "Scott", EnrollmentDate = DateTime.Parse("1998-12-12 12:46:57") },
                    new Student{ GovernmentId = "43456783", FirstMidName = "Jacob", LastName = "Green", EnrollmentDate = DateTime.Parse("2023-04-25 10:22:13") },
                    new Student{ GovernmentId = "56789016", FirstMidName = "Rachel", LastName = "Adams", EnrollmentDate = DateTime.Parse("2007-09-08 11:57:28") },
                    new Student{ GovernmentId = "50123459", FirstMidName = "Nicholas", LastName = "Baker", EnrollmentDate = DateTime.Parse("2016-02-20 12:31:44") },
                    new Student{ GovernmentId = "44567895", FirstMidName = "Kimberly", LastName = "Gonzalez", EnrollmentDate = DateTime.Parse("2003-07-14 10:14:59") },
                    new Student{ GovernmentId = "58901237", FirstMidName = "Jonathan", LastName = "Nelson", EnrollmentDate = DateTime.Parse("2012-12-26 11:42:15") },
                    new Student{ GovernmentId = "42345682", FirstMidName = "Lauren", LastName = "Carter", EnrollmentDate = DateTime.Parse("2000-05-09 12:19:31") },
                    new Student{ GovernmentId = "55678906", FirstMidName = "Brittany", LastName = "Perez", EnrollmentDate = DateTime.Parse("1997-03-05 11:34:02") },
                    new Student{ GovernmentId = "49012349", FirstMidName = "Austin", LastName = "Roberts", EnrollmentDate = DateTime.Parse("2008-08-18 12:08:18") },
                    new Student{ GovernmentId = "53456784", FirstMidName = "Victoria", LastName = "Turner", EnrollmentDate = DateTime.Parse("2018-01-30 10:45:34") },
                    new Student{ GovernmentId = "46789017", FirstMidName = "Zachary", LastName = "Phillips", EnrollmentDate = DateTime.Parse("2005-06-13 11:23:49") },
                    new Student{ GovernmentId = "40123460", FirstMidName = "Christina", LastName = "Campbell", EnrollmentDate = DateTime.Parse("2013-11-26 12:58:05") },
                    new Student{ GovernmentId = "54567896", FirstMidName = "Ethan", LastName = "Parker", EnrollmentDate = DateTime.Parse("2002-04-09 10:19:21") },
                    new Student{ GovernmentId = "47890128", FirstMidName = "Danielle", LastName = "Evans", EnrollmentDate = DateTime.Parse("2020-09-21 11:52:36") },
                    new Student{ GovernmentId = "51234572", FirstMidName = "Nathan", LastName = "Edwards", EnrollmentDate = DateTime.Parse("1999-02-03 12:27:52") },
                    new Student{ GovernmentId = "45678907", FirstMidName = "Amber", LastName = "Collins", EnrollmentDate = DateTime.Parse("2009-07-17 10:04:08") },
                    new Student{ GovernmentId = "59012350", FirstMidName = "Kyle", LastName = "Stewart", EnrollmentDate = DateTime.Parse("2017-12-30 11:41:23") },
                    new Student{ GovernmentId = "43456785", FirstMidName = "Alexis", LastName = "Sanchez", EnrollmentDate = DateTime.Parse("2004-05-12 12:15:39") },
                    new Student{ GovernmentId = "56789018", FirstMidName = "Cody", LastName = "Morris", EnrollmentDate = DateTime.Parse("2014-10-25 10:52:54") },
                    new Student{ GovernmentId = "50123461", FirstMidName = "Courtney", LastName = "Rogers", EnrollmentDate = DateTime.Parse("2001-03-08 11:30:10") },
                    new Student{ GovernmentId = "44567897", FirstMidName = "Aaron", LastName = "Reed", EnrollmentDate = DateTime.Parse("2011-08-21 12:06:26") },
                    new Student{ GovernmentId = "58901239", FirstMidName = "Kayla", LastName = "Cook", EnrollmentDate = DateTime.Parse("1998-01-04 10:43:41") },
                    new Student{ GovernmentId = "42345684", FirstMidName = "Jordan", LastName = "Morgan", EnrollmentDate = DateTime.Parse("2019-06-17 11:21:57") },
                    new Student{ GovernmentId = "55678908", FirstMidName = "Kristen", LastName = "Bell", EnrollmentDate = DateTime.Parse("2006-11-29 12:54:12") },
                    new Student{ GovernmentId = "49012351", FirstMidName = "Dylan", LastName = "Murphy", EnrollmentDate = DateTime.Parse("2015-04-13 10:28:28") },
                    new Student{ GovernmentId = "53456786", FirstMidName = "Taylor", LastName = "Bailey", EnrollmentDate = DateTime.Parse("2003-09-26 11:05:44") },
                    new Student{ GovernmentId = "46789019", FirstMidName = "Morgan", LastName = "Rivera", EnrollmentDate = DateTime.Parse("2012-02-08 12:39:59") },
                    new Student{ GovernmentId = "40123462", FirstMidName = "Travis", LastName = "Cooper", EnrollmentDate = DateTime.Parse("2000-07-22 10:17:15") },
                    new Student{ GovernmentId = "54567898", FirstMidName = "Hannah", LastName = "Richardson", EnrollmentDate = DateTime.Parse("2021-12-04 11:54:31") },
                    new Student{ GovernmentId = "47890130", FirstMidName = "Adam", LastName = "Cox", EnrollmentDate = DateTime.Parse("1997-05-18 12:28:46") },
                    new Student{ GovernmentId = "51234574", FirstMidName = "Sydney", LastName = "Howard", EnrollmentDate = DateTime.Parse("2008-10-31 10:06:02") },
                    new Student{ GovernmentId = "45678909", FirstMidName = "Lucas", LastName = "Ward", EnrollmentDate = DateTime.Parse("2018-03-15 11:43:18") },
                    new Student{ GovernmentId = "59012352", FirstMidName = "Allison", LastName = "Torres", EnrollmentDate = DateTime.Parse("2005-08-28 12:17:33") },
                    new Student{ GovernmentId = "43456787", FirstMidName = "Mason", LastName = "Peterson", EnrollmentDate = DateTime.Parse("2013-01-10 10:54:49") },
                    new Student{ GovernmentId = "56789020", FirstMidName = "Olivia", LastName = "Gray", EnrollmentDate = DateTime.Parse("2002-06-24 11:32:05") },
                    new Student{ GovernmentId = "50123463", FirstMidName = "Logan", LastName = "Ramirez", EnrollmentDate = DateTime.Parse("2020-11-06 12:09:20") },
                    new Student{ GovernmentId = "44567899", FirstMidName = "Emma", LastName = "James", EnrollmentDate = DateTime.Parse("1999-04-20 10:46:36") },
                    new Student{ GovernmentId = "58901241", FirstMidName = "Connor", LastName = "Watson", EnrollmentDate = DateTime.Parse("2010-09-02 11:23:52") },
                    new Student{ GovernmentId = "42345686", FirstMidName = "Sophia", LastName = "Brooks", EnrollmentDate = DateTime.Parse("2007-02-15 12:58:07") },
                    new Student{ GovernmentId = "55678910", FirstMidName = "Liam", LastName = "Kelly", EnrollmentDate = DateTime.Parse("2016-07-29 10:35:23") },
                    new Student{ GovernmentId = "49012353", FirstMidName = "Isabella", LastName = "Sanders", EnrollmentDate = DateTime.Parse("2004-12-11 11:12:39") },
                    new Student{ GovernmentId = "53456788", FirstMidName = "Noah", LastName = "Price", EnrollmentDate = DateTime.Parse("2014-05-24 12:47:54") },
                    new Student{ GovernmentId = "46789021", FirstMidName = "Ava", LastName = "Bennett", EnrollmentDate = DateTime.Parse("2001-10-07 10:25:10") },
                    new Student{ GovernmentId = "40123464", FirstMidName = "William", LastName = "Wood", EnrollmentDate = DateTime.Parse("2011-03-21 11:02:26") },
                    new Student{ GovernmentId = "54567900", FirstMidName = "Mia", LastName = "Barnes", EnrollmentDate = DateTime.Parse("1998-08-04 12:36:41") },
                    new Student{ GovernmentId = "47890132", FirstMidName = "Benjamin", LastName = "Ross", EnrollmentDate = DateTime.Parse("2019-01-17 10:13:57") },
                    new Student{ GovernmentId = "51234576", FirstMidName = "Charlotte", LastName = "Henderson", EnrollmentDate = DateTime.Parse("2006-06-30 11:51:13") },
                    new Student{ GovernmentId = "45678911", FirstMidName = "Elijah", LastName = "Coleman", EnrollmentDate = DateTime.Parse("2015-12-13 12:25:28") },
                    new Student{ GovernmentId = "59012354", FirstMidName = "Amelia", LastName = "Jenkins", EnrollmentDate = DateTime.Parse("2003-05-27 10:02:44") },
                    new Student{ GovernmentId = "43456789", FirstMidName = "Oliver", LastName = "Perry", EnrollmentDate = DateTime.Parse("2012-10-09 11:40:00") },
                    new Student{ GovernmentId = "56789022", FirstMidName = "Harper", LastName = "Powell", EnrollmentDate = DateTime.Parse("2000-03-23 12:14:15") },
                    new Student{ GovernmentId = "50123465", FirstMidName = "Henry", LastName = "Long", EnrollmentDate = DateTime.Parse("2021-08-05 10:51:31") },
                    new Student{ GovernmentId = "44567901", FirstMidName = "Evelyn", LastName = "Patterson", EnrollmentDate = DateTime.Parse("1997-01-19 11:28:47") },
                    new Student{ GovernmentId = "58901243", FirstMidName = "Alexander", LastName = "Hughes", EnrollmentDate = DateTime.Parse("2009-06-03 12:03:02") },
                    new Student{ GovernmentId = "42345688", FirstMidName = "Abigail", LastName = "Flores", EnrollmentDate = DateTime.Parse("2017-11-16 10:40:18") },
                    new Student{ GovernmentId = "55678912", FirstMidName = "Sebastian", LastName = "Washington", EnrollmentDate = DateTime.Parse("2005-04-30 11:17:34") },
                    new Student{ GovernmentId = "49012355", FirstMidName = "Emily", LastName = "Butler", EnrollmentDate = DateTime.Parse("2013-09-12 12:51:49") },
                    new Student{ GovernmentId = "53456790", FirstMidName = "Jackson", LastName = "Simmons", EnrollmentDate = DateTime.Parse("2002-02-26 10:29:05") },
                    new Student{ GovernmentId = "46789023", FirstMidName = "Ella", LastName = "Foster", EnrollmentDate = DateTime.Parse("2020-07-10 11:06:21") },
                    new Student{ GovernmentId = "40123466", FirstMidName = "Aiden", LastName = "Gonzales", EnrollmentDate = DateTime.Parse("1999-12-23 12:40:36") }
                ];
        }
    }
}
