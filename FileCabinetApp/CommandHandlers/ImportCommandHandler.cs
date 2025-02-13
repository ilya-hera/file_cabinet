﻿using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FileCabinetApp.Services;
using FileCabinetApp.Services.SnapshotServices;
using FileCabinetApp.Validators;
using FileCabinetApp.Validators.InputValidators;

namespace FileCabinetApp.CommandHandlers
{
    /// <summary>
    /// Class <c>ImportCommandHandler</c> implement import command.
    /// </summary>
    /// <seealso cref="CabinetServiceCommandHandlerBase" />
    internal class ImportCommandHandler : CabinetServiceCommandHandlerBase
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="ImportCommandHandler"/> class.
        /// </summary>
        /// <param name="fileCabinetService">The file cabinet service.</param>
        public ImportCommandHandler(IFileCabinetService fileCabinetService)
            : base(fileCabinetService)
        {
        }

        /// <summary>
        /// Handles the specified application command request.
        /// </summary>
        /// <param name="appCommandRequest">The application command request.</param>
        /// <exception cref="System.ArgumentNullException">appCommandRequest.</exception>
        public override void Handle(AppCommandRequest appCommandRequest)
        {
            if (appCommandRequest == null)
            {
                throw new ArgumentNullException(nameof(appCommandRequest));
            }

            if (appCommandRequest.Command.Equals("import", StringComparison.OrdinalIgnoreCase))
            {
                var parametersTuple = InputValidator.ValidateImportExportParameters(appCommandRequest.Parameters);

                try
                {
                    this.TryReadRecords(parametersTuple);
                }
                catch (Exception e) when (e is IOException || e is ArgumentNullException || e is ArgumentException)
                {
                    Console.WriteLine(e.Message);
                    return;
                }

                Console.WriteLine($"All records were imported from file {parametersTuple.Item2!.Split('\\')[^1]}");
                return;
            }

            base.Handle(appCommandRequest);
        }

        private bool TryReadRecords(Tuple<bool, string, string> parametersTuple)
        {
            IFileCabinetServiceSnapshot snapshot = this.FileCabinetService.MakeSnapshot();
            if (parametersTuple.Item2 is null)
            {
                Console.WriteLine("File path can not be null");
                return false;
            }

            using FileStream fs = new FileStream(parametersTuple.Item2, FileMode.Open, FileAccess.Read);
            if (parametersTuple.Item1 && parametersTuple.Item3.Equals("csv", StringComparison.OrdinalIgnoreCase))
            {
                using StreamReader streamReader = new StreamReader(fs);
                snapshot.LoadFromCsv(streamReader);
                this.FileCabinetService.Restore(snapshot);
            }
            else if (parametersTuple.Item1 && parametersTuple.Item3.Equals("xml", StringComparison.OrdinalIgnoreCase))
            {
                using StreamReader streamReader = new StreamReader(fs);
                snapshot.LoadFromXml(streamReader);
                if (snapshot.Records.Count == 0)
                {
                    return false;
                }

                this.FileCabinetService.Restore(snapshot);
            }
            else
            {
                return false;
            }

            return true;
        }
    }
}
