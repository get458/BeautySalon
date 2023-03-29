using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using BeautySalon;
using System.Linq;

namespace BeautySalonTest
{
    [TestClass]
    public class BeautySalonTest
    {
        [TestMethod]
        public void AuthorizationTest()
        {
            var EmployeeTest = DB.db.Employee.FirstOrDefault(x => x.Login == "Test");
            AuthorizationPage authorizationPage = new AuthorizationPage();
            Assert.IsFalse(authorizationPage.Autorization("", ""));
            Assert.IsFalse(authorizationPage.Autorization("Tests", "QWEasd123"));
            Assert.IsFalse(authorizationPage.Autorization("Test", "123"));
            EmployeeTest.IsWorking = false;
            DB.db.SaveChanges();
            Assert.IsFalse(authorizationPage.Autorization("Test", "Test"));
            Assert.IsFalse(authorizationPage.Autorization("\n", "\r"));
            EmployeeTest.IsWorking = true;
            DB.db.SaveChanges();
            Assert.IsTrue(authorizationPage.Autorization("Test", "Test"));

        }

        [TestMethod]
        public void AddClientTest()
        {
            App app = new App();
            app.InitializeComponent();
            Person Test = new Person
            {
                LastName = "",
                Name = "",
                MiddleName = ""
            };

            Client TestClient = new Client
            {
                Person = Test,
                IDDiscount = 1,
                Phone = ""
            };

            WorkOfClientPage workOfClientPage = new WorkOfClientPage();
            Assert.IsFalse(workOfClientPage.AddClient(TestClient));
            TestClient.Person.LastName = "TestClient1";
            TestClient.Person.Name = "TestClient";
            TestClient.Person.MiddleName = "TestClient";
            TestClient.Phone = "qwe";
            Assert.IsFalse(workOfClientPage.AddClient(TestClient));
            TestClient.Person.LastName = "TestClient";
            Assert.IsFalse(workOfClientPage.AddClient(TestClient));
            TestClient.Phone = "+79539153874";
            Assert.IsTrue(workOfClientPage.AddClient(TestClient));
            var FindClient = DB.db.Client.FirstOrDefault(x => x.Person.LastName == "TestClient");
            Assert.IsNotNull(FindClient);
            //Очистка данных
            var FindPerson = DB.db.Person.FirstOrDefault(x => x.LastName == "TestClient");
            DB.db.Client.Remove(FindClient);
            DB.db.Person.Remove(FindPerson);
            DB.db.SaveChanges();
        }   

        [TestMethod]
        public void EditClientTest()
        {
            Person Test = new Person
            {
                LastName = "TestClient",
                Name = "TestClient",
                MiddleName = "TestClient"
            };

            Client TestClient = new Client
            {
                Person = Test,
                IDDiscount = 1,
                Phone = "+79539153874"
            };
            WorkOfClientPage workOfClientPage = new WorkOfClientPage();
            DB.db.Client.Add(TestClient);
            DB.db.SaveChanges();
            workOfClientPage.DataContext = TestClient;
            TestClient.Person.LastName = "TestClient1";
            Assert.IsFalse(workOfClientPage.EditClient());
            TestClient.Person.LastName = "TestClient";
            TestClient.Phone = "qwe";
            Assert.IsFalse(workOfClientPage.EditClient());
            TestClient.Phone = "+79539153874";
            Assert.IsTrue(workOfClientPage.EditClient());
            var FindClient = DB.db.Client.FirstOrDefault(x => x.Person.LastName == "TestClient");
            Assert.IsNotNull(FindClient);
            //Очистка данных
            var FindPerson = DB.db.Person.FirstOrDefault(x => x.LastName == "TestClient");
            DB.db.Client.Remove(FindClient);
            DB.db.Person.Remove(FindPerson);
            DB.db.SaveChanges();
        }

        [TestMethod]
        public void DeleteClientTest()
        {
            Person Test = new Person
            {
                LastName = "TestClient",
                Name = "TestClient",
                MiddleName = "TestClient"
            };

            Client TestClient = new Client
            {
                Person = Test,
                IDDiscount = 1,
                Phone = "+79539153874"
            };
            DB.db.Client.Add(TestClient);
            DB.db.SaveChanges();
            ClientPage clientPage = new ClientPage();
            //Нажать нет
            Assert.IsFalse(clientPage.DeleteClient(TestClient));
            Assert.IsTrue(clientPage.DeleteClient(TestClient));
            var FindClient = DB.db.Client.FirstOrDefault(x => x.Person.LastName == "TestClient");
            Assert.IsNull(FindClient);
            //Очистка данных
            var FindPerson = DB.db.Person.FirstOrDefault(x => x.LastName == "TestClient");
            DB.db.Person.Remove(FindPerson);
            DB.db.SaveChanges();
        }

        [TestMethod]
        public void TotalCostTest()
        {
            RecordingPage recordingPage = new RecordingPage();
            ProcedureOfMaster Cost550 = DB.db.ProcedureOfMaster.Find(1);
            ProcedureOfMaster Cost1300 = DB.db.ProcedureOfMaster.Find(2);
            int Discount = 0;
            decimal ExpectedResult = 1850;
            recordingPage.ProceduresInRecording
                .Add(new RecordingPage.TempDataOfRecording
                { ProcedureMaster = Cost550, Time = new TimeSpan(1, 1, 0) });
            recordingPage.ProceduresInRecording
                .Add(new RecordingPage.TempDataOfRecording
                { ProcedureMaster = Cost1300, Time = new TimeSpan(1, 1, 0) });
            Assert.AreEqual(ExpectedResult, recordingPage.TotalCost(Discount));
            Assert.AreEqual(0, recordingPage.TotalCost(Discount));
            Assert.AreEqual(-1, recordingPage.TotalCost(Discount));
        }

        [TestMethod]
        public void AddProcedureTest()
        {
            Procedure TestProcedure = new Procedure()
            {
                ProceduresName = "",
                Cost = -12
            };
            WorkOfProcedurePage workOfProcedurePage = new WorkOfProcedurePage();
            Assert.IsFalse(workOfProcedurePage.AddProcedure(TestProcedure));
            TestProcedure.ProceduresName = "TestProcedure";
            TestProcedure.Cost = 0;
            Assert.IsFalse(workOfProcedurePage.AddProcedure(TestProcedure));
            TestProcedure.Cost = 100;
            Assert.IsTrue(workOfProcedurePage.AddProcedure(TestProcedure));
            var FindProcedure = DB.db.Procedure.FirstOrDefault(x => x.ProceduresName == "TestProcedure");
            Assert.IsNotNull(FindProcedure);
            DB.db.Procedure.Remove(FindProcedure);
            DB.db.SaveChanges();
        }

        [TestMethod]
        public void EditProcedureTest()
        {
            Procedure TestProcedure = new Procedure()
            {
                ProceduresName = "StartTestProcedure",
                Cost = 100
            };
            DB.db.Procedure.Add(TestProcedure);
            DB.db.SaveChanges();
            WorkOfProcedurePage workOfProcedurePage = new WorkOfProcedurePage();
            workOfProcedurePage.DataContext = TestProcedure;
            TestProcedure.ProceduresName = "";
            Assert.IsFalse(workOfProcedurePage.EditProcedure());
            TestProcedure.ProceduresName = "TestProcedure";
            TestProcedure.Cost = 0;
            Assert.IsFalse(workOfProcedurePage.EditProcedure());
            TestProcedure.Cost = 100;
            Assert.IsTrue(workOfProcedurePage.EditProcedure());
            var FindProcedure = DB.db.Procedure.FirstOrDefault(x => x.ProceduresName == "TestProcedure");
            Assert.IsNotNull(FindProcedure);
            DB.db.Procedure.Remove(FindProcedure);
            DB.db.SaveChanges();
        }


        [TestMethod]
        public void DeleteProcedureTest()
        {
            Procedure TestProcedure = new Procedure()
            {
                ProceduresName = "TestProcedure",
                Cost = 100
            };
            DB.db.Procedure.Add(TestProcedure);
            DB.db.SaveChanges();
            ProcedurePage ProcedurePage = new ProcedurePage();
            //Нажать нет
            Assert.IsFalse(ProcedurePage.DeleteProcedure(TestProcedure));
            Assert.IsTrue(ProcedurePage.DeleteProcedure(TestProcedure));
            var FindProcedure = DB.db.Procedure.FirstOrDefault(x => x.ProceduresName == "TestProcedure");
            Assert.IsNull(FindProcedure);
        }

        [TestMethod]
        public void CanceledRecordingTest()
        {
            ProcedureInRecording TestRecording = DB.db.ProcedureInRecording.Find(1);
            ScheduleMasterPage scheduleMasterPage = new ScheduleMasterPage();
            //Нажать нет
            Assert.IsFalse(scheduleMasterPage.CanceledProcedure(TestRecording));
            Assert.IsTrue(scheduleMasterPage.CanceledProcedure(TestRecording));
            TestRecording.IsCanceled = false;
            DB.db.SaveChanges();
        }

        [TestMethod]
        public void DismissialEmployeeTest()
        {
            var EmployeeTest = DB.db.Employee.FirstOrDefault(x => x.Login == "Test");
            EmployeePage scheduleMasterPage = new EmployeePage();
            //Нажать нет
            Assert.IsFalse(scheduleMasterPage.DismissialEmployee(EmployeeTest));
            Assert.IsTrue(scheduleMasterPage.DismissialEmployee(EmployeeTest));
            EmployeeTest.IsWorking = true;
            DB.db.SaveChanges();
        }
    }
}
