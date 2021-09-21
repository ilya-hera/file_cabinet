﻿using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using FileCabinetApp.Entities;
using FileCabinetApp.Services.SnapshotServices;
using FileCabinetApp.Utils;
using FileCabinetApp.Validators;
using Microsoft.Win32.SafeHandles;

namespace FileCabinetApp.Services.FileService
{
    /// <summary>
    /// Class <c>FileCabinetFilesystemService</c>.
    /// </summary>
    /// <seealso cref="FileCabinetApp.Services.IFileCabinetService" />
    public class FileCabinetFilesystemService : IFileCabinetService
    {
        private const int NameSizes = 120;

        private const int StringsInFile = 2;

        private const int RecordSize = sizeof(int) // id
                                       + (NameSizes * 2) // First name + Last name
                                       + (sizeof(int) * 3)
                                       + sizeof(short) // balance
                                       + sizeof(decimal) // money
                                       + sizeof(char) // account type
                                       + StringsInFile; // String's count because the number in front of each string tell how many bytes are necessary to store the string in binary file

        private readonly FileStream fileStream;

        private readonly IRecordValidator validator = new DefaultValidator();

        private int recordsCount;

        /// <summary>
        /// Initializes a new instance of the <see cref="FileCabinetFilesystemService"/> class.
        /// </summary>
        /// <param name="fileStream">The file stream.</param>
        public FileCabinetFilesystemService(FileStream fileStream)
        {
            this.fileStream = fileStream;
        }

        /// <summary>
        /// Creates the record.
        /// </summary>
        /// <param name="container">The record of parameters.</param>
        /// <returns>
        /// Return Id of created record.
        /// </returns>
        public int CreateRecord(ParametersContainer container)
        {
            this.validator.ValidateParameters(container);
            var offset = this.fileStream.Length;
            this.recordsCount += 1;
            var id = this.recordsCount;

            if (container != null)
            {
                this.WriteRecord(offset, new FileCabinetRecord(container, id));
            }

            return id;
        }

        /// <summary>
        /// Gets all records in File Cabinet.
        /// </summary>
        /// <returns>
        /// Return array of <c>FileCabinetRecord</c>.
        /// </returns>
        /// <exception cref="System.NotImplementedException">Not implemented.</exception>
        public IReadOnlyCollection<FileCabinetRecord> GetRecords()
        {
            var resultArray = this.recordsCount == 0 ? Array.Empty<FileCabinetRecord>() : new FileCabinetRecord[this.recordsCount];
            var count = 0;

            for (int i = 0; count < this.recordsCount; i++, count++)
            {
                byte[] buffer = new byte[RecordSize];
                this.fileStream.Seek(RecordSize * i, SeekOrigin.Begin);
                this.fileStream.Read(buffer);
                var status = BitConverter.ToInt16(buffer.AsSpan()[0..2]);
                if (status == 1)
                {
                    count--;
                    continue;
                }

                resultArray[count] = new FileCabinetRecord
                {
                    Id = BitConverter.ToInt32(buffer.AsSpan()[2..6]),
                    FirstName = ByteConverter.ToString(buffer[6..126]),
                    LastName = ByteConverter.ToString(buffer[126..246]),
                    DateOfBirth = ByteConverter.ToDateTime(buffer[246..258]),
                    WorkingHoursPerWeek = BitConverter.ToInt16(buffer.AsSpan()[258..260]),
                    AnnualIncome = ByteConverter.ToDecimal(buffer[260..276]),
                    DriverLicenseCategory = BitConverter.ToChar(buffer.AsSpan()[276..278]),
                };
            }

            return resultArray;
        }

        /// <summary>
        /// Edits the record.
        /// </summary>
        /// <param name="id">The identifier.</param>
        /// <param name="container">The record of parameters.</param>
        /// <exception cref="System.NotImplementedException">Not implemented.</exception>
        public void EditRecord(int id, ParametersContainer container)
        {
            this.validator.ValidateParameters(container);

            long offset = RecordSize * (id - 1);
            byte[] buffer = new byte[RecordSize];

            this.fileStream.Seek(offset, SeekOrigin.Begin);
            this.fileStream.Write(buffer);

            if (container != null)
            {
                this.WriteRecord(offset, new FileCabinetRecord(container, id));
            }
        }

        /// <summary>
        /// Gets the stat of users.
        /// </summary>
        /// <returns>
        /// Return count of records in File Cabinet.
        /// </returns>
        /// <exception cref="System.NotImplementedException">Not implemented.</exception>
        public int GetStat()
        {
            return this.recordsCount;
        }

        /// <summary>
        /// Finds the records by the first name.
        /// </summary>
        /// <param name="firstName">The first name.</param>
        /// <returns>
        /// Return array of records.
        /// </returns>
        /// <exception cref="System.NotImplementedException">Not implemented.</exception>
        public IReadOnlyCollection<FileCabinetRecord> FindByFirstName(string firstName)
        {
            var records = this.GetRecords();
            var filteredRecords = records.Where(record =>
                firstName.Equals(record.FirstName, StringComparison.OrdinalIgnoreCase)).ToArray();

            return filteredRecords;
        }

        /// <summary>
        /// Finds the records by the last name.
        /// </summary>
        /// <param name="lastName">The last name.</param>
        /// <returns>
        /// Return array of records.
        /// </returns>
        /// <exception cref="System.NotImplementedException">Not implemented.</exception>
        public IReadOnlyCollection<FileCabinetRecord> FindByLastName(string lastName)
        {
            var records = this.GetRecords();
            var filteredRecords = records.Where(record =>
                lastName.Equals(record.LastName, StringComparison.OrdinalIgnoreCase)).ToArray();

            return filteredRecords;
        }

        /// <summary>
        /// Finds the records by date of birthday.
        /// </summary>
        /// <param name="dateOfBirth">The date of birthday.</param>
        /// <returns>
        /// Return array of records.
        /// </returns>
        /// <exception cref="System.NotImplementedException">Not implemented.</exception>
        public IReadOnlyCollection<FileCabinetRecord> FindByDateOfBirthName(DateTime dateOfBirth)
        {
            var records = this.GetRecords();
            var filteredRecords = records.Where(record => dateOfBirth == record.DateOfBirth).ToArray();

            return filteredRecords;
        }

        /// <summary>
        /// Makes the snapshot.
        /// </summary>
        /// <returns>
        /// Return <c>FileCabinetServiceSnapshot</c>.
        /// </returns>
        /// <exception cref="System.NotImplementedException">Not implemented.</exception>
        public IFileCabinetServiceSnapshot MakeSnapshot()
        {
            var snapshot = new FileCabinetServiceSnapshot(this.GetRecords().ToArray(), this.validator);

            return snapshot;
        }

        /// <summary>
        /// Restores this instance.
        /// </summary>
        /// <param name="snapshot">IFileCabinetServiceSnapshot.</param>
        /// <exception cref="System.NotImplementedException">Not implemented.</exception>
        public void Restore(IFileCabinetServiceSnapshot snapshot)
        {
            if (snapshot == null)
            {
                throw new ArgumentException("Snapshot can't be null");
            }

            if (this.fileStream != null)
            {
                List<FileCabinetRecord> snapShotRecords = snapshot.Records.ToList();
                this.recordsCount = snapShotRecords.Count;
                byte[] buffer = new byte[this.recordsCount * RecordSize];

                this.fileStream.Seek(0, SeekOrigin.Begin);
                this.fileStream.Write(buffer);
                foreach (var record in snapShotRecords)
                {
                    this.WriteRecord((record.Id - 1) * RecordSize, record);
                }
            }
        }

        /// <summary>
        /// Removes the record from FileCabinetApp.
        /// </summary>
        /// <param name="id">The identifier.</param>
        public void RemoveRecord(int id)
        {
            if (id < 1)
            {
                throw new ArgumentException("The value should be greater than zero.", nameof(id));
            }

            if (this.recordsCount < 1)
            {
                throw new ArgumentException("There are no records.");
            }

            try
            {
                if (this.fileStream != null)
                {
                    for (int i = 0; i < this.recordsCount; i++)
                    {
                        byte[] buffer = new byte[RecordSize];
                        this.fileStream.Read(buffer);
                        this.fileStream.Seek(i * RecordSize, SeekOrigin.Begin);

                        short status = BitConverter.ToInt16(buffer.AsSpan()[0..2]);

                        if (status == 1)
                        {
                            continue;
                        }

                        if (BitConverter.ToInt32(buffer.AsSpan()[2..6]) == id)
                        {
                            this.fileStream.Seek((i - 1) * RecordSize, SeekOrigin.Begin);
                            this.fileStream.Write(BitConverter.GetBytes((short)1)); // 0 - not deleted, 1 - deleted
                            this.recordsCount--;
                            return;
                        }
                    }
                }
            }
            catch (IOException ex)
            {
                Console.WriteLine(ex.Message);
            }
        }

        private void WriteRecord(long offset,  FileCabinetRecord record)
        {
            this.fileStream.Seek(offset, SeekOrigin.Begin);
            this.fileStream.Write(BitConverter.GetBytes((short)0)); // Id
            this.fileStream.Seek(offset + 2, SeekOrigin.Begin);
            this.fileStream.Write(BitConverter.GetBytes(record.Id)); // Id
            this.fileStream.Seek(offset + 6, SeekOrigin.Begin);
            this.fileStream.Write(Encoding.GetEncoding("UTF-8").GetBytes(record.FirstName.ToCharArray())); // first name
            this.fileStream.Seek(offset + 126, SeekOrigin.Begin);
            this.fileStream.Write(Encoding.GetEncoding("UTF-8").GetBytes(record.LastName.ToCharArray())); // last name
            this.fileStream.Seek(offset + 246, SeekOrigin.Begin);
            this.fileStream.Write(BitConverter.GetBytes(record.DateOfBirth.Year)); // year
            this.fileStream.Seek(offset + 250, SeekOrigin.Begin);
            this.fileStream.Write(BitConverter.GetBytes(record.DateOfBirth.Month)); // month
            this.fileStream.Seek(offset + 254, SeekOrigin.Begin);
            this.fileStream.Write(BitConverter.GetBytes(record.DateOfBirth.Day)); // day
            this.fileStream.Seek(offset + 258, SeekOrigin.Begin);
            this.fileStream.Write(BitConverter.GetBytes(record.WorkingHoursPerWeek)); // working hours
            this.fileStream.Seek(offset + 260, SeekOrigin.Begin);
            this.fileStream.Write(ByteConverter.GetBytes(record.AnnualIncome)); // annual income
            this.fileStream.Seek(offset + 276, SeekOrigin.Begin);
            this.fileStream.Write(BitConverter.GetBytes(record.DriverLicenseCategory)); // Driver license category
            this.fileStream.Seek(offset + 278, SeekOrigin.Begin);
        }
    }
}
