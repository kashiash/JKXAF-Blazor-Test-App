﻿

using DevExpress.Data.Filtering;
using DevExpress.ExpressApp;
using DevExpress.ExpressApp.Security;
using DevExpress.ExpressApp.Updating;
using DevExpress.Persistent.Base;
using DevExpress.Persistent.Base.General;
using DevExpress.Persistent.BaseImpl.PermissionPolicy;
using JKXAF.Module.BusinessObjects;
using JKXAF.Module.Utils;
using System;

namespace JKXAF.Module.DatabaseUpdate
{
    // For more typical usage scenarios, be sure to check out https://docs.devexpress.com/eXpressAppFramework/DevExpress.ExpressApp.Updating.ModuleUpdater
    public class Updater : ModuleUpdater
    {
        public Updater(IObjectSpace objectSpace, Version currentDBVersion) :
            base(objectSpace, currentDBVersion)
        {
        }
        public override void UpdateDatabaseAfterUpdateSchema()
        {
            base.UpdateDatabaseAfterUpdateSchema();

            var watch = new System.Diagnostics.Stopwatch();

            watch.Start();

            //Create departments.




            Department devDepartment = ObjectSpace.FindObject<Department>(CriteriaOperator.Parse("Title == 'R&D'"));
            if (devDepartment == null)
            {
                devDepartment = ObjectSpace.CreateObject<Department>();
                devDepartment.Title = "R&D";
                devDepartment.Office = "1";
                devDepartment.Save();
            }
            Department supDepartment = ObjectSpace.FindObject<Department>(CriteriaOperator.Parse("Title == 'Technical Support'"));
            if (supDepartment == null)
            {
                supDepartment = ObjectSpace.CreateObject<Department>();
                supDepartment.Title = "Technical Support";
                supDepartment.Office = "2";
                supDepartment.Save();
            }
            Department mngDepartment = ObjectSpace.FindObject<Department>(CriteriaOperator.Parse("Title == 'Management'"));
            if (mngDepartment == null)
            {
                mngDepartment = ObjectSpace.CreateObject<Department>();
                mngDepartment.Title = "Management";
                mngDepartment.Office = "3";
                mngDepartment.Save();
            }
            //Create employees.
            //Admin is a god that can do everything.
            Employee administrator = ObjectSpace.FindObject<Employee>(CriteriaOperator.Parse("UserName == 'Admin'"));
            if (administrator == null)
            {
                administrator = ObjectSpace.CreateObject<Employee>();
                administrator.UserName = "Admin";
                administrator.FirstName = "Admin";
                administrator.LastName = "Admin";
                administrator.Department = mngDepartment;
                administrator.IsActive = true;
                administrator.SetPassword(string.Empty);
                administrator.Roles.Add(GetAdministratorRole());
                administrator.Save();
            }
            //Sam is a manager and he can do everything with his own department
            Employee managerSam = ObjectSpace.FindObject<Employee>(CriteriaOperator.Parse("UserName == 'Sam'"));
            if (managerSam == null)
            {
                managerSam = ObjectSpace.CreateObject<Employee>();
                managerSam.UserName = "Sam";
                managerSam.FirstName = "Sam";
                managerSam.LastName = "Jackson";
                managerSam.IsActive = true;
                managerSam.SetPassword(string.Empty);
                managerSam.Department = devDepartment;
                managerSam.Roles.Add(GetManagerRole());
                managerSam.Save();
            }
            //John is an ordinary user within the Sam's department.
            Employee userJohn = ObjectSpace.FindObject<Employee>(CriteriaOperator.Parse("UserName == 'John'"));
            if (userJohn == null)
            {
                userJohn = ObjectSpace.CreateObject<Employee>();
                userJohn.UserName = "John";
                userJohn.FirstName = "John";
                userJohn.LastName = "Doe";
                userJohn.IsActive = true;
                userJohn.SetPassword(string.Empty);
                userJohn.Department = devDepartment;
                userJohn.Roles.Add(GetUserRole());
                userJohn.Save();
            }
            //Mary is a manager of another department.  
            Employee managerMary = ObjectSpace.FindObject<Employee>(CriteriaOperator.Parse("UserName == 'Mary'"));
            if (managerMary == null)
            {
                managerMary = ObjectSpace.CreateObject<Employee>();
                managerMary.UserName = "Mary";
                managerMary.FirstName = "Mary";
                managerMary.LastName = "Tellinson";
                managerMary.IsActive = true;
                managerMary.SetPassword(string.Empty);
                managerMary.Department = supDepartment;
                managerMary.Roles.Add(GetManagerRole());
                managerMary.Save();
            }
            //Joe is an ordinary user within the Mary's department.
            Employee userJoe = ObjectSpace.FindObject<Employee>(CriteriaOperator.Parse("UserName == 'Joe'"));
            if (userJoe == null)
            {
                userJoe = ObjectSpace.CreateObject<Employee>();
                userJoe.UserName = "Joe";
                userJoe.FirstName = "Joe";
                userJoe.LastName = "Pitt";
                userJoe.IsActive = true;
                userJoe.SetPassword(string.Empty);
                userJoe.Department = supDepartment;
                userJoe.Roles.Add(GetUserRole());
                userJoe.Save();
            }
            Employee userJason = ObjectSpace.FindObject<Employee>(CriteriaOperator.Parse("UserName == 'Joe'"));
            if (userJoe == null)
            {
                userJoe = ObjectSpace.CreateObject<Employee>();
                userJoe.UserName = "Jason";
                userJoe.FirstName = "Joe";
                userJoe.LastName = "Connor";
                userJoe.IsActive = true;
                userJoe.SetPassword(string.Empty);
                userJoe.Department = supDepartment;
                userJoe.Roles.Add(GetUserRole());
                userJoe.Save();
            }
            //Create tasks for employees.
            if (ObjectSpace.FindObject<EmployeeTask>(CriteriaOperator.Parse("Subject == 'Do homework'")) == null)
            {
                EmployeeTask task = ObjectSpace.CreateObject<EmployeeTask>();
                task.Subject = "Do homework";
                task.AssignedTo = managerSam;
                task.DueDate = DateTime.Now;
                task.Status = TaskStatus.NotStarted;
                task.Description = "This is a task for Sam";
                task.Save();
            }
            for (int i = 0; i < 99; i++)
            {


                if (ObjectSpace.FindObject<EmployeeTask>(CriteriaOperator.Parse($"Subject == 'Prepare {i} coffee for everyone'")) == null)
                {
                    EmployeeTask task = ObjectSpace.CreateObject<EmployeeTask>();
                    task.Subject = $"Prepare {i} coffee for everyone";
                    task.AssignedTo = userJohn;
                    task.DueDate = DateTime.Now;
                    task.Status = TaskStatus.InProgress;
                    task.Description = "This is a task for John";
                    task.Save();
                }

                if (ObjectSpace.FindObject<EmployeeTask>(CriteriaOperator.Parse($"Subject == 'Read {i} latest news'")) == null)
                {
                    EmployeeTask task = ObjectSpace.CreateObject<EmployeeTask>();
                    task.Subject = $"Read {i} latest news";
                    task.AssignedTo = managerMary;
                    task.DueDate = DateTime.Now;
                    task.Status = TaskStatus.Completed;
                    task.Description = "This is a task for Mary";
                    task.Save();
                }
                if (ObjectSpace.FindObject<EmployeeTask>(CriteriaOperator.Parse($"Subject == 'Book {i} tickets'")) == null)
                {
                    EmployeeTask task = ObjectSpace.CreateObject<EmployeeTask>();
                    task.Subject = $"Book {i} tickets";
                    task.AssignedTo = userJoe;
                    task.DueDate = DateTime.Now;
                    task.Status = TaskStatus.Deferred;
                    task.Description = "This is a task for Joe";
                    task.Save();
                }
            }

            ObjectSpace.CommitChanges();

            DataGeneratorHelper.GenerateCountries(ObjectSpace);
            DataGeneratorHelper.GenerateCustomers(ObjectSpace, 100);
            ObjectSpace.CommitChanges();
            DataGeneratorHelper.GenerateInvoices(ObjectSpace, 1000);

            ObjectSpace.CommitChanges();



            watch.Stop();

            Console.WriteLine($"Update Execution Time: {watch.ElapsedMilliseconds} ms");


            //string name = "MyName";
            //DomainObject1 theObject = ObjectSpace.FindObject<DomainObject1>(CriteriaOperator.Parse("Name=?", name));
            //if(theObject == null) {
            //    theObject = ObjectSpace.CreateObject<DomainObject1>();
            //    theObject.Name = name;
            //}

            //ObjectSpace.CommitChanges(); //Uncomment this line to persist created object(s).
        }



        public override void UpdateDatabaseBeforeUpdateSchema()
        {
            base.UpdateDatabaseBeforeUpdateSchema();

        }


        private PermissionPolicyRole GetAdministratorRole()
        {
            PermissionPolicyRole administratorRole = ObjectSpace.FindObject<PermissionPolicyRole>(new BinaryOperator("Name", "Administrators"));
            if (administratorRole == null)
            {
                administratorRole = ObjectSpace.CreateObject<PermissionPolicyRole>();
                administratorRole.Name = "Administrators";
                //Can access everything.
                administratorRole.IsAdministrative = true;
            }
            return administratorRole;
        }
        //Users can access and partially edit data (no create and delete capabilities) from their own department.
        private PermissionPolicyRole GetUserRole()
        {
            PermissionPolicyRole userRole = ObjectSpace.FindObject<PermissionPolicyRole>(new BinaryOperator("Name", "Users"));
            if (userRole == null)
            {
                userRole = ObjectSpace.CreateObject<PermissionPolicyRole>();
                userRole.Name = "Users";

                userRole.AddNavigationPermission("Application/NavigationItems/Items/Default/Items/MyDetails", SecurityPermissionState.Allow);
                userRole.AddNavigationPermission("Application/NavigationItems/Items/Default/Items/Employee_ListView", SecurityPermissionState.Allow);
                userRole.AddNavigationPermission("Application/NavigationItems/Items/Default/Items/EmployeeTask_ListView", SecurityPermissionState.Allow);
                userRole.AddNavigationPermission("Application/NavigationItems/Items/Default/Items/Customer_ListView", SecurityPermissionState.Allow);
                userRole.AddNavigationPermission("Application/NavigationItems/Items/Default/Items/Invoice_ListView", SecurityPermissionState.Allow);

                userRole.AddObjectPermission<Employee>(SecurityOperations.Read, "Department.Employees[Oid = CurrentUserId()]", SecurityPermissionState.Allow);
                userRole.AddMemberPermission<Employee>(SecurityOperations.Write, "ChangePasswordOnFirstLogon;StoredPassword;FirstName;LastName", "Oid=CurrentUserId()", SecurityPermissionState.Allow);
                userRole.AddMemberPermission<Employee>(SecurityOperations.Write, "Tasks", "Department.Employees[Oid = CurrentUserId()]", SecurityPermissionState.Allow);

                userRole.SetTypePermission<PermissionPolicyRole>(SecurityOperations.Read, SecurityPermissionState.Allow);

                userRole.AddObjectPermission<EmployeeTask>(SecurityOperations.ReadWriteAccess, "AssignedTo.Department.Employees[Oid = CurrentUserId()]", SecurityPermissionState.Allow);
                userRole.AddMemberPermission<EmployeeTask>(SecurityOperations.Write, "AssignedTo", "AssignedTo.Department.Employees[Oid = CurrentUserId()]", SecurityPermissionState.Allow);

                userRole.AddObjectPermission<Department>(SecurityOperations.Read, "Employees[Oid=CurrentUserId()]", SecurityPermissionState.Allow);
           
                userRole.AddObjectPermission<Invoice>(SecurityOperations.ReadWriteAccess, "[Klient] Is Not Null And [Klient.Consultant.Oid] = CurrentUserId()", SecurityPermissionState.Allow);

                userRole.AddObjectPermission<Customer>(SecurityOperations.ReadWriteAccess, "[Consultant.Oid] = CurrentUserId()", SecurityPermissionState.Allow);
                userRole.SetTypePermission<Department>(SecurityOperations.Read, SecurityPermissionState.Allow);
                userRole.SetTypePermission<InvoiceItem>(SecurityOperations.Read, SecurityPermissionState.Allow);
                userRole.SetTypePermission<Product>(SecurityOperations.Read, SecurityPermissionState.Allow);
            }
            return userRole;
        }
        //Managers can access and fully edit (including create and delete capabilities) data from their own department. However, they cannot access data from other departments.
        private PermissionPolicyRole GetManagerRole()
        {
            PermissionPolicyRole managerRole = ObjectSpace.FindObject<PermissionPolicyRole>(new BinaryOperator("Name", "Managers"));
            if (managerRole == null)
            {
                managerRole = ObjectSpace.CreateObject<PermissionPolicyRole>();
                managerRole.Name = "Managers";

                managerRole.AddNavigationPermission("Application/NavigationItems/Items/Default/Items/MyDetails", SecurityPermissionState.Allow);
                managerRole.AddNavigationPermission("Application/NavigationItems/Items/Default/Items/Department_ListView", SecurityPermissionState.Allow);
                managerRole.AddNavigationPermission("Application/NavigationItems/Items/Default/Items/Employee_ListView", SecurityPermissionState.Allow);
                managerRole.AddNavigationPermission("Application/NavigationItems/Items/Default/Items/EmployeeTask_ListView", SecurityPermissionState.Allow);
                managerRole.AddNavigationPermission("Application/NavigationItems/Items/Default/Items/Customer_ListView", SecurityPermissionState.Allow);
                managerRole.AddNavigationPermission("Application/NavigationItems/Items/Default/Items/Invoice_ListView", SecurityPermissionState.Allow);


                managerRole.AddObjectPermission<Department>(SecurityOperations.FullObjectAccess, "Employees[Oid=CurrentUserId()]", SecurityPermissionState.Allow);

                managerRole.SetTypePermission<Employee>(SecurityOperations.Create, SecurityPermissionState.Allow);
                managerRole.AddObjectPermission<Employee>(SecurityOperations.FullObjectAccess, "IsNull(Department) || Department.Employees[Oid=CurrentUserId()]", SecurityPermissionState.Allow);

                managerRole.SetTypePermission<EmployeeTask>(SecurityOperations.Create, SecurityPermissionState.Allow);
                managerRole.AddObjectPermission<EmployeeTask>(SecurityOperations.FullObjectAccess,
                    "IsNull(AssignedTo) || IsNull(AssignedTo.Department) || AssignedTo.Department.Employees[Oid=CurrentUserId()]", SecurityPermissionState.Allow);

                managerRole.SetTypePermission<PermissionPolicyRole>(SecurityOperations.Read, SecurityPermissionState.Allow);

                managerRole.SetTypePermission<Department>(SecurityOperations.Read, SecurityPermissionState.Allow);
                managerRole.SetTypePermission<InvoiceItem>(SecurityOperations.Read, SecurityPermissionState.Allow);
                managerRole.SetTypePermission<Product>(SecurityOperations.Read, SecurityPermissionState.Allow);
                managerRole.AddObjectPermission<Invoice>(SecurityOperations.ReadWriteAccess, "[Klient] Is Null Or [Klient.Consultant] Is Null Or [Klient.Consultant.Department.Employees][[Oid] = CurrentUserId()]", SecurityPermissionState.Allow);

                managerRole.AddObjectPermission<Customer>(SecurityOperations.ReadWriteAccess, "[Consultant] Is Null Or [Consultant.Department.Employees][[Oid] = CurrentUserId()]", SecurityPermissionState.Allow);


                
            }
            return managerRole;
        }

    }
}
