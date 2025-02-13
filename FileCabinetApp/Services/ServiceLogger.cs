﻿using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using FileCabinetApp.Entities;
using FileCabinetApp.Services.SnapshotServices;

namespace FileCabinetApp.Services
{
    /// <summary>
    /// Class <c>ServiceLogger</c> make logging of commands.
    /// </summary>
    /// <seealso cref="FileCabinetApp.Services.IFileCabinetService" />
    public class ServiceLogger : IFileCabinetService
    {
        private readonly IFileCabinetService fileCabinetService;

        private readonly string path = Path.Combine(Directory.GetCurrentDirectory(), "Log.txt");

        /// <summary>
        /// Initializes a new instance of the <see cref="ServiceLogger"/> class.
        /// </summary>
        /// <param name="fileCabinetService">The file cabinet service implementation.</param>
        public ServiceLogger(IFileCabinetService fileCabinetService)
        {
            this.fileCabinetService = fileCabinetService;
        }

        private static string TimeString => $"{DateTime.Now.ToShortDateString()} {DateTime.Now.ToShortTimeString()}  -  ";

        /// <summary>
        /// Creates the record.
        /// </summary>
        /// <param name="container">The container of parameters.</param>
        /// <returns>
        /// Return Id of created record.
        /// </returns>
        public int CreateRecord(ParametersContainer container)
        {
            int result;

            var sb = new StringBuilder();
            sb.Append($"{DateTime.Now.ToShortDateString()} {DateTime.Now.ToShortTimeString()}  -  ");
            sb.Append($"Calling {GetCurrentMethod()} with FirstName = '{container?.FirstName} ', ");
            sb.Append($"LastName = '{container?.LastName}', ");
            sb.Append($"DateOfBirth = '{container?.DateOfBirthday.ToShortDateString()}', ");
            sb.Append($"Working hours per week = '{container?.WorkingHoursPerWeek}', ");
            sb.Append($"Annual income = '{container?.AnnualIncome}', ");
            sb.Append($"Driver license category = '{container?.DriverLicenseCategory}'.");

            using (var fs = new FileStream(this.path, FileMode.Append, FileAccess.Write))
            using (var writer = new StreamWriter(fs, Encoding.UTF8))
            {
                writer.WriteLine(sb.ToString());

                if (this.fileCabinetService is ServiceMeter meter)
                {
                    result = meter.CreateRecord(container);
                }
                else
                {
                    result = this.fileCabinetService.CreateRecord(container);
                }

                writer.WriteLine($"{DateTime.Now.ToShortDateString()} {DateTime.Now.ToShortTimeString()}  -  {GetCurrentMethod()} returned '{result}'");
            }

            return result;
        }

        /// <summary>
        /// Edits the record.
        /// </summary>
        /// <param name="id">The identifier.</param>
        /// <param name="container">The container of parameters.</param>
        public void EditRecord(int id, ParametersContainer container)
        {
            var sb = new StringBuilder();
            sb.Append($"{DateTime.Now.ToShortDateString()} {DateTime.Now.ToShortTimeString()}  -  ");
            sb.Append($"Calling {GetCurrentMethod()} with id = '{id} ', ");
            sb.Append($"FirstName = '{container?.FirstName} ', ");
            sb.Append($"LastName = '{container?.LastName}', ");
            sb.Append($"DateOfBirth = '{container?.DateOfBirthday.ToShortDateString()}', ");
            sb.Append($"Working hours per week = '{container?.WorkingHoursPerWeek}', ");
            sb.Append($"Annual income = '{container?.AnnualIncome}', ");
            sb.Append($"Driver license category = '{container?.DriverLicenseCategory}'.");

            using (var fs = new FileStream(this.path, FileMode.Append, FileAccess.Write))
            using (var writer = new StreamWriter(fs, Encoding.UTF8))
            {
                writer.WriteLine(sb.ToString());

                if (this.fileCabinetService is ServiceMeter meter)
                {
                    meter.EditRecord(id, container);
                }
                else
                {
                    this.fileCabinetService.EditRecord(id, container);
                }
            }
        }

        /// <summary>
        /// Inserts the record.
        /// </summary>
        /// <param name="container">The container.</param>
        /// <returns>
        /// Return is record inserted.
        /// </returns>
        public bool InsertRecord(ParametersContainer container)
        {
            var sb = new StringBuilder();
            sb.Append($"{DateTime.Now.ToShortDateString()} {DateTime.Now.ToShortTimeString()}  -  ");
            sb.Append($"Calling {GetCurrentMethod()} with id = '{container?.Id} ', ");
            sb.Append($"FirstName = '{container?.FirstName} ', ");
            sb.Append($"LastName = '{container?.LastName}', ");
            sb.Append($"DateOfBirth = '{container?.DateOfBirthday.ToShortDateString()}', ");
            sb.Append($"Working hours per week = '{container?.WorkingHoursPerWeek}', ");
            sb.Append($"Annual income = '{container?.AnnualIncome}', ");
            sb.Append($"Driver license category = '{container?.DriverLicenseCategory}'.");

            var result = true;
            using (var fs = new FileStream(this.path, FileMode.Append, FileAccess.Write))
            using (var writer = new StreamWriter(fs, Encoding.UTF8))
            {
                writer.WriteLine(sb.ToString());

                if (this.fileCabinetService is ServiceMeter meter)
                {
                    result = meter.InsertRecord(container);
                }
                else
                {
                    result = this.fileCabinetService.InsertRecord(container);
                }
            }

            return result;
        }

        /// <summary>
        /// Gets the stat of users.
        /// </summary>
        /// <returns>
        /// Return count of records and deleted records in File Cabinet.
        /// </returns>
        public Tuple<int, int> GetStat()
        {
            Tuple<int, int> result;

            var sb = new StringBuilder();
            sb.Append($"{DateTime.Now.ToShortDateString()} {DateTime.Now.ToShortTimeString()}  -  ");
            sb.Append($"Calling {GetCurrentMethod()}. ");

            using (var fs = new FileStream(this.path, FileMode.Append, FileAccess.Write))
            using (var writer = new StreamWriter(fs, Encoding.UTF8))
            {
                writer.WriteLine(sb.ToString());

                if (this.fileCabinetService is ServiceMeter meter)
                {
                    result = meter.GetStat();
                }
                else
                {
                    result = this.fileCabinetService.GetStat();
                }

                writer.WriteLine($"{DateTime.Now.ToShortDateString()} {DateTime.Now.ToShortTimeString()}  -  {GetCurrentMethod()} returned '{result.Item1}, {result.Item2}'");
            }

            return result;
        }

        /// <summary>
        /// Finds the by identifier.
        /// </summary>
        /// <param name="id">The identifier.</param>
        /// <returns>
        /// Return the FileCabinetRecord.
        /// </returns>
        public IEnumerable<FileCabinetRecord> FindById(int id)
        {
            IEnumerable<FileCabinetRecord> result;
            var sb = new StringBuilder();
            sb.Append($"{DateTime.Now.ToShortDateString()} {DateTime.Now.ToShortTimeString()}  -  ");
            sb.Append($"Calling {GetCurrentMethod()} with Id = '{id} ', ");

            using (var fs = new FileStream(this.path, FileMode.Append, FileAccess.Write))
            using (var writer = new StreamWriter(fs, Encoding.UTF8))
            {
                writer.WriteLine(sb.ToString());

                if (this.fileCabinetService is ServiceMeter meter)
                {
                    result = meter.FindById(id);
                }
                else
                {
                    result = this.fileCabinetService.FindById(id);
                }

                writer.WriteLine($"{DateTime.Now.ToShortDateString()} {DateTime.Now.ToShortTimeString()}  -  {GetCurrentMethod()} returned:");
            }

            return result;
        }

        /// <summary>
        /// Finds the records by the first name.
        /// </summary>
        /// <param name="value">The first name.</param>
        /// <returns>
        /// Return array of records.
        /// </returns>
        public IEnumerable<FileCabinetRecord> FindByFirstName(string value)
        {
            IEnumerable<FileCabinetRecord> result;
            var sb = new StringBuilder();
            sb.Append($"{DateTime.Now.ToShortDateString()} {DateTime.Now.ToShortTimeString()}  -  ");
            sb.Append($"Calling {GetCurrentMethod()} with FirstName = '{value} ', ");

            using (var fs = new FileStream(this.path, FileMode.Append, FileAccess.Write))
            using (var writer = new StreamWriter(fs, Encoding.UTF8))
            {
                writer.WriteLine(sb.ToString());

                if (this.fileCabinetService is ServiceMeter meter)
                {
                    result = meter.FindByFirstName(value);
                }
                else
                {
                    result = this.fileCabinetService.FindByFirstName(value);
                }

                writer.WriteLine($"{DateTime.Now.ToShortDateString()} {DateTime.Now.ToShortTimeString()}  -  {GetCurrentMethod()} returned:");
                writer.Write(WriteRecords(result));
            }

            return result;
        }

        /// <summary>
        /// Finds the records by the last name.
        /// </summary>
        /// <param name="value">The last name.</param>
        /// <returns>
        /// Return array of records.
        /// </returns>
        public IEnumerable<FileCabinetRecord> FindByLastName(string value)
        {
            IEnumerable<FileCabinetRecord> result;
            var sb = new StringBuilder();
            sb.Append($"{DateTime.Now.ToShortDateString()} {DateTime.Now.ToShortTimeString()}  -  ");
            sb.Append($"Calling {GetCurrentMethod()} with LastName = '{value} ', ");

            using (var fs = new FileStream(this.path, FileMode.Append, FileAccess.Write))
            using (var writer = new StreamWriter(fs, Encoding.UTF8))
            {
                writer.WriteLine(sb.ToString());

                if (this.fileCabinetService is ServiceMeter meter)
                {
                    result = meter.FindByLastName(value);
                }
                else
                {
                    result = this.fileCabinetService.FindByLastName(value);
                }

                writer.WriteLine($"{DateTime.Now.ToShortDateString()} {DateTime.Now.ToShortTimeString()}  -  {GetCurrentMethod()} returned:");
                writer.Write(WriteRecords(result));
            }

            return result;
        }

        /// <summary>
        /// Finds the records by date of birthday.
        /// </summary>
        /// <param name="value">The date of birthday.</param>
        /// <returns>
        /// Return array of records.
        /// </returns>
        public IEnumerable<FileCabinetRecord> FindByDateOfBirthday(DateTime value)
        {
            IEnumerable<FileCabinetRecord> result;
            var sb = new StringBuilder();
            sb.Append($"{DateTime.Now.ToShortDateString()} {DateTime.Now.ToShortTimeString()}  -  ");
            sb.Append($"Calling {GetCurrentMethod()} with birthDay = '{value} ', ");

            using (var fs = new FileStream(this.path, FileMode.Append, FileAccess.Write))
            using (var writer = new StreamWriter(fs, Encoding.UTF8))
            {
                writer.WriteLine(sb.ToString());

                if (this.fileCabinetService is ServiceMeter meter)
                {
                    result = meter.FindByDateOfBirthday(value);
                }
                else
                {
                    result = this.fileCabinetService.FindByDateOfBirthday(value);
                }

                writer.WriteLine($"{DateTime.Now.ToShortDateString()} {DateTime.Now.ToShortTimeString()}  -  {GetCurrentMethod()} returned:");
                writer.Write(WriteRecords(result));
            }

            return result;
        }

        /// <summary>
        /// Finds the records by the last name.
        /// </summary>
        /// <param name="workingHours">The working hours.</param>
        /// <returns>
        /// Return array of records.
        /// </returns>
        public IEnumerable<FileCabinetRecord> FindByWorkingHours(short workingHours)
        {
            IEnumerable<FileCabinetRecord> result;
            var sb = new StringBuilder();
            sb.Append($"{DateTime.Now.ToShortDateString()} {DateTime.Now.ToShortTimeString()}  -  ");
            sb.Append($"Calling {GetCurrentMethod()} with birthDay = '{workingHours} ', ");

            using (var fs = new FileStream(this.path, FileMode.Append, FileAccess.Write))
            using (var writer = new StreamWriter(fs, Encoding.UTF8))
            {
                writer.WriteLine(sb.ToString());

                if (this.fileCabinetService is ServiceMeter meter)
                {
                    result = meter.FindByWorkingHours(workingHours);
                }
                else
                {
                    result = this.fileCabinetService.FindByWorkingHours(workingHours);
                }

                writer.WriteLine($"{DateTime.Now.ToShortDateString()} {DateTime.Now.ToShortTimeString()}  -  {GetCurrentMethod()} returned:");
                writer.Write(WriteRecords(result));
            }

            return result;
        }

        /// <summary>
        /// Finds the records by the last name.
        /// </summary>
        /// <param name="annualIncome">The annual income.</param>
        /// <returns>
        /// Return array of records.
        /// </returns>
        public IEnumerable<FileCabinetRecord> FindByAnnualIncome(decimal annualIncome)
        {
            IEnumerable<FileCabinetRecord> result;
            var sb = new StringBuilder();
            sb.Append($"{DateTime.Now.ToShortDateString()} {DateTime.Now.ToShortTimeString()}  -  ");
            sb.Append($"Calling {GetCurrentMethod()} with birthDay = '{annualIncome} ', ");

            using (var fs = new FileStream(this.path, FileMode.Append, FileAccess.Write))
            using (var writer = new StreamWriter(fs, Encoding.UTF8))
            {
                writer.WriteLine(sb.ToString());

                if (this.fileCabinetService is ServiceMeter meter)
                {
                    result = meter.FindByAnnualIncome(annualIncome);
                }
                else
                {
                    result = this.fileCabinetService.FindByAnnualIncome(annualIncome);
                }

                writer.WriteLine($"{DateTime.Now.ToShortDateString()} {DateTime.Now.ToShortTimeString()}  -  {GetCurrentMethod()} returned:");
                writer.Write(WriteRecords(result));
            }

            return result;
        }

        /// <summary>
        /// Finds the records by the last name.
        /// </summary>
        /// <param name="driverCategory">The driver category.</param>
        /// <returns>
        /// Return array of records.
        /// </returns>
        public IEnumerable<FileCabinetRecord> FindByDriverCategory(char driverCategory)
        {
            IEnumerable<FileCabinetRecord> result;
            var sb = new StringBuilder();
            sb.Append($"{DateTime.Now.ToShortDateString()} {DateTime.Now.ToShortTimeString()}  -  ");
            sb.Append($"Calling {GetCurrentMethod()} with birthDay = '{driverCategory} ', ");

            using (var fs = new FileStream(this.path, FileMode.Append, FileAccess.Write))
            using (var writer = new StreamWriter(fs, Encoding.UTF8))
            {
                writer.WriteLine(sb.ToString());

                if (this.fileCabinetService is ServiceMeter meter)
                {
                    result = meter.FindByDriverCategory(driverCategory);
                }
                else
                {
                    result = this.fileCabinetService.FindByDriverCategory(driverCategory);
                }

                writer.WriteLine($"{DateTime.Now.ToShortDateString()} {DateTime.Now.ToShortTimeString()}  -  {GetCurrentMethod()} returned:");
                writer.Write(WriteRecords(result));
            }

            return result;
        }

        /// <summary>
        /// Makes the snapshot.
        /// </summary>
        /// <returns>
        /// Return <c>FileCabinetServiceSnapshot</c>.
        /// </returns>
        public IFileCabinetServiceSnapshot MakeSnapshot()
        {
            IFileCabinetServiceSnapshot result;

            var sb = new StringBuilder();
            sb.Append(TimeString);
            sb.Append($"Calling {GetCurrentMethod()}. ");

            using (var fs = new FileStream(this.path, FileMode.Append, FileAccess.Write))
            using (var writer = new StreamWriter(fs, Encoding.UTF8))
            {
                writer.WriteLine(sb.ToString());

                if (this.fileCabinetService is ServiceMeter meter)
                {
                    result = meter.MakeSnapshot();
                }
                else
                {
                    result = this.fileCabinetService.MakeSnapshot();
                }

                writer.WriteLine($"{DateTime.Now.ToShortDateString()} {DateTime.Now.ToShortTimeString()}  -  {GetCurrentMethod()} returned:");

                writer.Write(WriteRecords(result.Records));
            }

            return result;
        }

        /// <summary>
        /// Restores the specified snapshot.
        /// </summary>
        /// <param name="snapshot">The snapshot.</param>
        public void Restore(IFileCabinetServiceSnapshot snapshot)
        {
            var sb = new StringBuilder();
            sb.Append($"{DateTime.Now.ToShortDateString()} {DateTime.Now.ToShortTimeString()}  -  ");
            sb.Append($"Calling {GetCurrentMethod()}. With snapshot records: ");

            using (var fs = new FileStream(this.path, FileMode.Append, FileAccess.Write))
            using (var writer = new StreamWriter(fs, Encoding.UTF8))
            {
                writer.WriteLine(sb.ToString());

                writer.Write(WriteRecords(snapshot?.Records));

                if (this.fileCabinetService is ServiceMeter meter)
                {
                    meter.Restore(snapshot);
                }
                else
                {
                    this.fileCabinetService.Restore(snapshot);
                }
            }
        }

        /// <summary>
        /// Gets all records in File Cabinet.
        /// </summary>
        /// <returns>
        /// Return array of <c>FileCabinetRecord</c>.
        /// </returns>
        public IReadOnlyCollection<FileCabinetRecord> GetRecords()
        {
            IReadOnlyCollection<FileCabinetRecord> result;

            var sb = new StringBuilder();
            sb.Append(TimeString);
            sb.Append($"Calling {GetCurrentMethod()}. ");

            using (var fs = new FileStream(this.path, FileMode.Append, FileAccess.Write))
            using (var writer = new StreamWriter(fs, Encoding.UTF8))
            {
                writer.WriteLine(sb.ToString());

                if (this.fileCabinetService is ServiceMeter meter)
                {
                    result = meter.GetRecords();
                }
                else
                {
                    result = this.fileCabinetService.GetRecords();
                }

                writer.WriteLine($"{DateTime.Now.ToShortDateString()} {DateTime.Now.ToShortTimeString()}  -  {GetCurrentMethod()} returned:");
                writer.Write(WriteRecords(result));
            }

            return result;
        }

        /// <summary>
        /// Removes the record from FileCabinetApp.
        /// </summary>
        /// <param name="id">The identifier.</param>
        public void RemoveRecord(int id)
        {
            var sb = new StringBuilder();
            sb.Append($"{DateTime.Now.ToShortDateString()} {DateTime.Now.ToShortTimeString()}  -  ");
            sb.Append($"Calling {GetCurrentMethod()} with id = '{id}'. ");

            using (var fs = new FileStream(this.path, FileMode.Append, FileAccess.Write))
            using (var writer = new StreamWriter(fs, Encoding.UTF8))
            {
                writer.WriteLine(sb.ToString());

                if (this.fileCabinetService is ServiceMeter meter)
                {
                    meter.RemoveRecord(id);
                }
                else
                {
                    this.fileCabinetService.RemoveRecord(id);
                }
            }
        }

        /// <summary>
        /// Removing voids in the data file formed by deleted records.
        /// </summary>
        /// <returns>
        /// Return purged Count.
        /// </returns>
        public int Purge()
        {
            int result;

            var sb = new StringBuilder();
            sb.Append(TimeString);
            sb.Append($"Calling {GetCurrentMethod()}. ");

            using (var fs = new FileStream(this.path, FileMode.Append, FileAccess.Write))
            using (var writer = new StreamWriter(fs, Encoding.UTF8))
            {
                writer.WriteLine(sb.ToString());

                if (this.fileCabinetService is ServiceMeter meter)
                {
                    result = meter.Purge();
                }
                else
                {
                    result = this.fileCabinetService.Purge();
                }

                writer.WriteLine($"{DateTime.Now.ToShortDateString()} {DateTime.Now.ToShortTimeString()}  -  {GetCurrentMethod()} returned '{result}'");
            }

            return result;
        }

        private static string WriteRecords(IEnumerable<FileCabinetRecord> records)
        {
            var sb = new StringBuilder();
            IEnumerable<FileCabinetRecord> fileCabinetRecords = records.ToList();
            if (!fileCabinetRecords.Any())
            {
                sb.AppendLine("no records.");
                return sb.ToString();
            }

            foreach (FileCabinetRecord record in fileCabinetRecords)
            {
                sb.Append($"#{record.Id}, ");
                sb.Append($"{record.FirstName}, ");
                sb.Append($"{record.LastName}, ");
                sb.Append($"{record.DateOfBirth.ToShortDateString()}, ");
                sb.Append($"driver license category: {record.DriverLicenseCategory}, ");
                sb.Append($"annual income: {record.AnnualIncome:F}$, ");
                sb.Append($"working hours: {record.WorkingHoursPerWeek}h.");
                sb.AppendLine();
            }

            return sb.ToString();
        }

        [MethodImpl(MethodImplOptions.NoInlining)]
        private static string GetCurrentMethod()
        {
            var st = new StackTrace();
            var sf = st.GetFrame(1);

            return sf.GetMethod().Name;
        }
    }
}
