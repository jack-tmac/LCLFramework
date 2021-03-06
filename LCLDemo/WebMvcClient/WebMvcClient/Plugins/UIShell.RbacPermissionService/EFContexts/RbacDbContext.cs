﻿using LCL;
using LCL.Repositories.EntityFramework;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using UIShell.RbacPermissionService;

namespace UIShell.RbacPermissionService
{
    public class RbacDbContext : BaseDbContext
    {
        public RbacDbContext(string nameOrConnectionString)
            : base(nameOrConnectionString)
        {

        }
        //RBAC
        public DbSet<Role> Role { get; set; }
        public DbSet<RoleAuthority> RoleAuthority { get; set; }
        public DbSet<User> User { get; set; }
        public DbSet<DictType> DictType { get; set; }
        public DbSet<Dictionary> Dictionary { get; set; }
        public DbSet<ScheduledEvents> ScheduledEvents { get; set; }
        public DbSet<TLog> TLog { get; set; }
        public DbSet<Xzqy> Xzqy { get; set; }

        public DbSet<UnitInfo> UnitInfo { get; set; }
        public DbSet<Department> Department { get; set; }
        public DbSet<CompanyInfo> CompanyInfo { get; set; }
        public DbSet<User_Employee> User_Employee { get; set; }
        public DbSet<SchoolInfo> SchoolInfo { get; set; }
        public DbSet<User_Teacher> User_Teacher { get; set; }
        public DbSet<User_TeacherCheck> User_TeacherCheck { get; set; }
        public DbSet<User_Student> User_Student { get; set; }
        
    }
}
