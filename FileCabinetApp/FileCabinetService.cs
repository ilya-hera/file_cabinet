﻿using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;

namespace FileCabinetApp
{
    /// <summary>
    /// Class <c>FileCabinetService</c> with File Cabinet.
    /// </summary>
    public class FileCabinetService
    {
        private readonly List<FileCabinetRecord> list = new List<FileCabinetRecord>();
        private readonly Dictionary<string, List<FileCabinetRecord>> firstNameDictionary = new Dictionary<string, List<FileCabinetRecord>>();
        private readonly Dictionary<string, List<FileCabinetRecord>> lastNameDictionary = new Dictionary<string, List<FileCabinetRecord>>();
        private readonly Dictionary<DateTime, List<FileCabinetRecord>> dateOfBirthDictionary = new Dictionary<DateTime, List<FileCabinetRecord>>();

        /// <summary>
        /// Creates the record.
        /// </summary>
        /// <param name="container">The container of parameters.</param>
        /// <returns>Return Id of created record.</returns>
        public int CreateRecord(ParametersContainer container)
        {
            if (container is null)
            {
                throw new ArgumentNullException(nameof(container));
            }

            this.CreateValidator().ValidateParameters(container);
            var record = new FileCabinetRecord
            {
                Id = this.list.Count + 1,
                FirstName = container.FirstName,
                LastName = container.LastName,
                DateOfBirth = container.DateOfBirthday,
                WorkingHoursPerWeek = container.WorkingHoursPerWeek,
                AnnualIncome = container.AnnualIncome,
                DriverLicenseCategory = container.DriverLicenseCategory,
            };

            this.list.Add(record);
            this.AddRecordToFirstNameDict(container.FirstName, record);
            this.AddRecordToLastNameDict(container.LastName, record);
            this.AddRecordToDateOfBirthDict(container.DateOfBirthday, record);

            return record.Id;
        }

        /// <summary>
        /// Edits the record.
        /// </summary>
        /// <param name="id">The identifier.</param>
        /// <param name="container">The container of parameters.</param>
        /// <exception cref="System.ArgumentException">#{id} record is not found.</exception>
        /// <exception cref="System.ArgumentNullException">container.</exception>
        public void EditRecord(int id, ParametersContainer container)
        {
            var record = this.list.FirstOrDefault(r => r.Id == id);
            if (record == null)
            {
                throw new ArgumentException($"#{id} record is not found.");
            }

            if (container is null)
            {
                throw new ArgumentNullException(nameof(container));
            }

            this.CreateValidator().ValidateParameters(container);

            this.EditFirstNameDictionary(container.FirstName, record);
            this.EditLastNameDictionary(container.LastName, record);
            this.EditDateOfBirthDictionary(container.DateOfBirthday, record);

            record.FirstName = container.FirstName;
            record.LastName = container.LastName;
            record.AnnualIncome = container.AnnualIncome;
            record.DateOfBirth = container.DateOfBirthday;
            record.DriverLicenseCategory = container.DriverLicenseCategory;
            record.WorkingHoursPerWeek = container.WorkingHoursPerWeek;
        }

        /// <summary>
        /// Gets all records in File Cabinet.
        /// </summary>
        /// <returns>Return array of <c>FileCabinetRecord</c>.</returns>
        public FileCabinetRecord[] GetRecords()
        {
            FileCabinetRecord[] resultArray = this.list.ToArray();
            return resultArray;
        }

        /// <summary>
        /// Gets the stat of users.
        /// </summary>
        /// <returns>Return count of records in File Cabinet.</returns>
        public int GetStat()
        {
            return this.list.Count;
        }

        /// <summary>
        /// Finds the records by the first name.
        /// </summary>
        /// <param name="firstName">The first name.</param>
        /// <returns>Return array of records.</returns>
        public FileCabinetRecord[] FindByFirstName(string firstName)
        {
            FileCabinetRecord[] resultArray = this.firstNameDictionary.ContainsKey(firstName) ? this.firstNameDictionary[firstName].ToArray() : Array.Empty<FileCabinetRecord>();

            return resultArray;
        }

        /// <summary>
        /// Finds the records by the last name.
        /// </summary>
        /// <param name="lastName">The last name.</param>
        /// <returns>Return array of records.</returns>
        public FileCabinetRecord[] FindByLastName(string lastName)
        {
            FileCabinetRecord[] resultArray = this.lastNameDictionary.ContainsKey(lastName) ? this.lastNameDictionary[lastName].ToArray() : Array.Empty<FileCabinetRecord>();

            return resultArray;
        }

        /// <summary>
        /// Finds the records by date of birthday.
        /// </summary>
        /// <param name="dateOfBirth">The date of birthday.</param>
        /// <returns>Return array of records.</returns>
        public FileCabinetRecord[] FindByDateOfBirthName(DateTime dateOfBirth)
        {
            FileCabinetRecord[] resultArray = this.dateOfBirthDictionary.ContainsKey(dateOfBirth) ? this.dateOfBirthDictionary[dateOfBirth].ToArray() : Array.Empty<FileCabinetRecord>();

            return resultArray;
        }

        /// <summary>
        /// Creates the validator.
        /// </summary>
        /// <returns>Return validator.</returns>
        protected virtual IRecordValidator CreateValidator()
        {
            return new DefaultValidator();
        }

        private void AddRecordToFirstNameDict(string firstName, FileCabinetRecord record)
        {
            if (this.firstNameDictionary.ContainsKey(firstName))
            {
                this.firstNameDictionary[firstName].Add(record);
            }
            else
            {
                this.firstNameDictionary.Add(firstName, new List<FileCabinetRecord>() { record });
            }
        }

        private void EditFirstNameDictionary(string firstName, FileCabinetRecord record)
        {
            if (!firstName.Equals(record.FirstName, StringComparison.OrdinalIgnoreCase))
            {
                this.firstNameDictionary[record.FirstName].Remove(record);
                if (this.firstNameDictionary[record.FirstName].Count == 0)
                {
                    this.firstNameDictionary.Remove(record.FirstName);
                }

                this.AddRecordToFirstNameDict(firstName, record);
            }
        }

        private void AddRecordToLastNameDict(string lastName, FileCabinetRecord record)
        {
            if (this.lastNameDictionary.ContainsKey(lastName))
            {
                this.lastNameDictionary[lastName].Add(record);
            }
            else
            {
                this.lastNameDictionary.Add(lastName, new List<FileCabinetRecord>() { record });
            }
        }

        private void EditLastNameDictionary(string lastName, FileCabinetRecord record)
        {
            if (!lastName.Equals(record.FirstName, StringComparison.OrdinalIgnoreCase))
            {
                this.lastNameDictionary[record.FirstName].Remove(record);
                if (this.lastNameDictionary[record.FirstName].Count == 0)
                {
                    this.lastNameDictionary.Remove(record.FirstName);
                }

                this.AddRecordToLastNameDict(lastName, record);
            }
        }

        private void AddRecordToDateOfBirthDict(DateTime dateOfBirth, FileCabinetRecord record)
        {
            if (this.dateOfBirthDictionary.ContainsKey(dateOfBirth))
            {
                this.dateOfBirthDictionary[dateOfBirth].Add(record);
            }
            else
            {
                this.dateOfBirthDictionary.Add(dateOfBirth, new List<FileCabinetRecord>() { record });
            }
        }

        private void EditDateOfBirthDictionary(DateTime dateOfBirth, FileCabinetRecord record)
        {
            if (dateOfBirth != record.DateOfBirth)
            {
                this.dateOfBirthDictionary[record.DateOfBirth].Remove(record);
                if (this.dateOfBirthDictionary[record.DateOfBirth].Count == 0)
                {
                    this.dateOfBirthDictionary.Remove(record.DateOfBirth);
                }

                this.AddRecordToDateOfBirthDict(dateOfBirth, record);
            }
        }
    }
}