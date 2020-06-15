using System;
using System.Collections.Generic;
using Bogus;
using Elo.Api.Dummy.Models;

namespace Elo.Api.Dummy.FakeData
{
    public class Generator
    {
        public IList<RequestPatientComplex> TestData;

        public Generator()
        {
            Execute();
        }

        private void Execute()
        {
            Randomizer.Seed = new Random(8415972);

            var assays = new[]
            {
                new Assay("Hep2", Technique.Ifa, "hep"),
                new Assay("Elisa", Technique.Elisa, "elisa"),
                new Assay("Blot", Technique.Blot, "blot"),
            };

            var testRequests = new Faker<RequestPatientComplex>()
                .StrictMode(false)
                .RuleFor(x => x.PatientBirthdate, f => f.Date.Past(80))
                .RuleFor(x => x.ArrivalDate, f => f.Date.Past())
                .Rules((faker, complex) =>
                {
                    var assay = faker.PickRandom(assays);
                    complex.AssayId = assay.Id;
                    complex.AssayShortname = assay.Shortname;
                    complex.Technique = assay.Technique;
                    complex.AssayToken = assay.Token;
                })
                .RuleFor(x => x.AssignedWorkstation, f => f.Hacker.Noun())
                .RuleFor(x => x.BillingType, f => f.Random.Enum<BillingType>())
                .RuleFor(x => x.Comment, f => f.Rant.Review())
                .RuleFor(x => x.BufferLotNumber, f => f.Random.Int(1000, 100000).ToString())
                .Rules((faker, complex) =>
                {
                    var qualResult = faker.Random.Enum<QualitativeRank>();
                    complex.QualitativeRank = qualResult;
                    complex.QuantitativeResult = faker.Random.Decimal(0.0M, 5.0M) + "iU/ml";
                    complex.CurrentDetailedResultString = qualResult.ToString("G") + "\n" + complex.QuantitativeResult;
                    complex.CurrentOfficialResultRemark = faker.Rant.Review(complex.AssayShortname);
                })
                .RuleFor(x => x.PatientFirstName, f => f.Person.FirstName)
                .RuleFor(x => x.PatientLastName, f => f.Person.LastName)
                .RuleFor(x => x.LifelongPatientId, f => f.Random.Int(10000, 100000).ToString())
                .RuleFor(x => x.PatientBirthdate, f => f.Person.DateOfBirth)
                .RuleFor(x => x.RequestCreatedOn, f => f.Date.Past())
                .RuleFor(x => x.RequestState, f => f.Random.Enum<RequestState>())
                .RuleFor(x => x.Priority, f => f.Random.Enum<RequestPriority>())
                .RuleFor(x => x.DueDate, f => f.Date.Future())
                .RuleFor(x => x.OrderNumber, f => f.IndexGlobal.ToString())
                .RuleFor(x => x.OrderableRunbasedLocation, f => f.IndexGlobal.ToString())
                .RuleFor(x => x.OrderState, f => f.Random.Enum<OrderState>())
                .RuleFor(x => x.ResultVerificationApprovedBy, f => f.Internet.UserName())
                .RuleFor(x => x.ResultVerificationApprovedOn, f => f.Date.Past())
                .RuleFor(x => x.ResultVerificationComment, f => f.Rant.Review())
                .RuleFor(x => x.SampleBarcode, f => f.Random.Int(10000, 100000).ToString())
                .RuleFor(x => x.SampleCategory, f => f.Random.Enum<SampleCategory>())
                .RuleFor(x => x.SampleLocation, f => f.IndexFaker.ToString())
                .RuleFor(x => x.Sex, f => f.Random.Enum<Sex>())
                .RuleFor(x => x.ArticleLotNumber, f => f.Random.Int(10000, 100000).ToString())
                .RuleFor(x => x.ConjugateLotNumber, f => f.Random.Int(10000, 100000).ToString())
                
                ;

            TestData = testRequests.Generate(2000);
        }

        class Assay
        {
            public Assay(string shortname, Technique technique, string token)
            {
                Id = Guid.NewGuid();
                Shortname = shortname;
                Technique = technique;
                Token = token;
            }

            public Guid Id { get; }
            public string Shortname { get; }
            public Technique Technique { get; }
            public string Token { get; }

        }
    }
}
