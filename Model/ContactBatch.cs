using System;
using System.Collections.Generic;
using System.IO;
using CsvHelper;
using FuzzyString;

namespace Waika.Model
{
    public class ContactBatch
    {
        private List<ContactInfo> _contacts;

        public List<ContactInfo> Contacts
        {
            get
            {
                if (_contacts == null)
                {
                    _contacts = new List<ContactInfo>();
                }
                return _contacts;
            }
            set { _contacts = value; }
        }
        public string ApiKey { get; set; }
        public Guid Id { get; private set; }
        public string ContentString { get; private set; }
        public List<Columns> HeaderOrder { get; private set; }

        public ContactBatch(string contentString, string apiKey)
        {
            Id = Guid.NewGuid();
            ContentString = contentString;
            ApiKey = apiKey;

            TextReader textReader = new StringReader(contentString);
            var csvParser = new CsvParser(textReader);

            var header = csvParser.Read();
            if (header == null)
            {
                throw new InvalidDataException();
            }
            HeaderOrder = GetHeaderOrder(header);

            while (true)
            {
                var row = csvParser.Read();
                if (row == null)
                {
                    break;
                }

                Contacts.Add(new ContactInfo(row, HeaderOrder));
            }
        }

        private static List<Columns> GetHeaderOrder(string[] header)
        {
            var options = new List<FuzzyStringComparisonOptions>
            {
                //FuzzyStringComparisonOptions.UseHammingDistance,
                FuzzyStringComparisonOptions.UseJaccardDistance,
                //FuzzyStringComparisonOptions.UseJaroDistance,
                //FuzzyStringComparisonOptions.UseJaroWinklerDistance,
                //FuzzyStringComparisonOptions.UseLevenshteinDistance,
                //FuzzyStringComparisonOptions.UseLongestCommonSubsequence,
                //FuzzyStringComparisonOptions.UseLongestCommonSubstring,
                //FuzzyStringComparisonOptions.UseNormalizedLevenshteinDistance,
                //FuzzyStringComparisonOptions.UseOverlapCoefficient,
                //FuzzyStringComparisonOptions.UseRatcliffObershelpSimilarity,
                //FuzzyStringComparisonOptions.UseSorensenDiceDistance,
                FuzzyStringComparisonOptions.UseTanimotoCoefficient
            };
            var tolerance = FuzzyStringComparisonTolerance.Normal;
            var headerOrder = new List<Columns>();
            for (int i = 0; i < header.Length; i++)
            {
                bool matchFound = false;
                foreach (var column in Enum.GetValues(typeof(Columns)))
                {
                    string enumName = Enum.GetName(typeof(Columns), column);
                    string headerName = header[i];
                    if (headerName.ApproximatelyEquals(enumName, options, tolerance))
                    {
                        if (matchFound)
                        {
                            throw new InvalidDataException("Wrong header names");
                        }
                        headerOrder.Add((Columns)column);
                        matchFound = true;
                    }
                }
                if (!matchFound)
                {
                    throw new InvalidDataException("Wrong header names");
                }
            }
            return headerOrder;
        }

        public string ToCsv()
        {
            TextWriter textWriter = new StringWriter();
            var csv = new CsvWriter(textWriter);
            foreach (var column in HeaderOrder)
            {
                csv.WriteField(column.ToString());
            }
            csv.NextRecord();
            foreach (var contact in Contacts)
            {
                foreach (var column in HeaderOrder)
                {
                    csv.WriteField(contact.GetFieldByColumn(column));
                }
                csv.NextRecord();
            }

            return textWriter.ToString();
        }
    }
}
