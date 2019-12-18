using System;
using System.Collections;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using FYP1.POSServiceReference;

namespace FYP1.Models
{
    public class EmployeeModel : ValidatableModel
    {
        private int _id;
        private string _firstName;
        private string _lastName;
        private string _fullname;
        private DateTime _birthdate;
        private string _stringbirthdate;
        private string _email;
        private string _phone;
        private string _pin;
        private string _password;
        private DateTime _joinDate;
        private string _stringjoinDate;
        private string _jobDescription;
        private double _salary;
        private int _absent;
        private double _dueNet;
        private int _rank;
        private int _status;
        private string _rankDesc;
        private string _statusDesc;

        private ObservableCollection<EmployeeModel> _employeesCollection;

        public int ID
        {
            get
            {
                return _id;
            }
            set
            {
                this.SetProperty(ref this._id, value);
            }
        }
        public string StatusDesc
        {
            get
            {
                return _statusDesc;
            }
            set
            {
                this.SetProperty(ref this._statusDesc, value);
            }
        }
        public string StringBirthdate
        {
            get
            {
                return _stringbirthdate;
            }
            set
            {
                this.SetProperty(ref this._stringbirthdate, value);
            }
        }
        public string StringJoinDate
        {
            get
            {
                return _stringjoinDate;
            }
            set
            {
                this.SetProperty(ref this._stringjoinDate, value);
            }
        }
        public string RankDesc
        {
            get
            {
                return _rankDesc;
            }
            set
            {
                this.SetProperty(ref this._rankDesc, value);
            }

        }

        [Required]
        public string FirstName
        {
            get
            {
                return _firstName;
            }
            set
            {
                this.SetProperty(ref this._firstName, value);
            }
        }

        public string FullName
        {
            get
            {
                return FirstName + " " + LastName;
            }
            set
            {
                this.SetProperty(ref this._fullname, value);
            }
        }
      

        [Required]
        [StringLength(4)]
        [CustomValidation(typeof(EmployeeModel), "checkPIN")]
        public string PIN
        {
            get
            {
                return _pin;
            }

            set { this.SetProperty(ref this._pin, value); }

        }
        [Required]
        public string Password
        {
            get
            {
                return _password;
            }

            set { this.SetProperty(ref this._password, value); }

        }
        public static ValidationResult checkPIN(object obj, ValidationContext context)
        {
            int? got = -1;
            var emp = (EmployeeModel)context.ObjectInstance;
            ServiceSoapClient webHandler = new ServiceSoapClient("ServiceSoap");
            webHandler.POS_Login(emp.PIN, ref got);
            if (got == 1)
            {
                return new ValidationResult("PIN already taken",
                    new List<string> { "PIN" });
            }
            return ValidationResult.Success;

        }

        [Required]
        [StringLength(20)]
        public string LastName
        {
            get
            {
                return _lastName;
            }

            set
            {
                this.SetProperty(ref this._lastName, value);
            }

        }
        [Required]
        public DateTime Birthdate
        {
            get
            {
                return _birthdate;
            }

            set { this.SetProperty(ref this._birthdate, value); }

        }
        [Required]
        [EmailAddress]
        public string Email
        {
            get
            {
                return _email;
            }

            set { this.SetProperty(ref this._email, value); }

        }
        [Required]
        [Phone]
        [StringLength(15)]
        public string Phone
        {
            get
            {
                return _phone;
            }

            set { this.SetProperty(ref this._phone, value); }

        }
        [Required]
        public DateTime JoinDate
        {
            get
            {
                return _joinDate;
            }

            set { this.SetProperty(ref this._joinDate, value); }

        }

        public string JobDescription
        {
            get
            {
                return _jobDescription;
            }

            set { this.SetProperty(ref this._jobDescription, value); }

        }
        [Required]
        public double Salary
        {
            get
            {
                return _salary;
            }

            set { this.SetProperty(ref this._salary, value); }

        }
        [Required]

        public int Absent
        {
            get
            {
                return _absent;
            }

            set { this.SetProperty(ref this._absent, value); }

        }
        [Required]

        public double DueNet
        {
            get
            {
                return _dueNet;
            }

            set { this.SetProperty(ref this._dueNet, value); }

        }
        [Required]

        public int Rank
        {
            get
            {
                return _rank;
            }

            set { this.SetProperty(ref this._rank, value); }

        }
        [Required]

        public int Status
        {
            get
            {
                return _status;
            }

            set { this.SetProperty(ref this._status, value); }

        }

        public ObservableCollection<EmployeeModel> EmployeesCollection
        {
            get
            {
                return _employeesCollection;
            }

            set { this.SetProperty(ref this._employeesCollection, value); }

        }
    }
}
