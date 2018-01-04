using StatMedClinic.Data.Mapping;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;

namespace StatMedClinic.API.Models
{
    public partial class StatMedClinicContext :DbContext
    {
        static StatMedClinicContext()
        {
            Database.SetInitializer<StatMedClinicContext>(null);
        }

        public StatMedClinicContext():base("Name=StatMedClinicContext")
        {
            
        }

        public DbSet<ChangePasswordBindingModel> ChangePasswordBindingModel { get; set; }
        public DbSet<LoginViewModel> LoginViewModel { get; set; }
        public DbSet<RegisterBindingModel> RegisterBindingModel { get; set; }
        public DbSet<SetPasswordBindingModel> SetPasswordBindingModel { get; set; }
        public DbSet<UserInfoViewModel> UserInfoViewModel { get; set; }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Configurations.Add(new UserDetailMapping());
            modelBuilder.Configurations.Add(new UserMap());
            modelBuilder.Configurations.Add(new UserStatuMap());
            modelBuilder.Configurations.Add(new UserTypeMap());
            modelBuilder.Configurations.Add(new ContactMap());
            modelBuilder.Configurations.Add(new ContactTypeMap());
            modelBuilder.Configurations.Add(new IndividualContactMap());
            modelBuilder.Configurations.Add(new IndividualMap());
            modelBuilder.Configurations.Add(new SalonContactMap());
            modelBuilder.Configurations.Add(new SchoolContactMap());
        }
    }
}