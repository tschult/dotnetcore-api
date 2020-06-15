using System;
using System.Collections.Generic;
using System.Text;

namespace Elo.Api.Dummy.Models
{
    /// <summary>
    /// Aggregates the data that is needed to display the RequestPatientOverview table.
    /// Each instance represents one row in the DataGrid.
    /// TODO: check whether we need QuantitativeResult, QualitativeResult, CurrentResult, and PreviousResult
    /// </summary>
    public class RequestPatientComplex
    {
        /// <summary>
        /// Arrival date of the sample at the laboratory
        /// </summary>
        public DateTime ArrivalDate { get; set; }

        public Guid AssayId { get; set; }

        /// <summary>
        /// Name of the assay (AssayShortname)
        /// </summary>
        public string AssayShortname { get; set; }

        /// <summary>
        /// Name of the assay in LIMS world (AssayToken)
        /// </summary>
        public string AssayToken { get; set; }

        public string AssignedWorkstation { get; set; }

        public Guid AssignedWorkstationId { get; set; }

        /// <summary>
        /// Billing type of the ifaRequest
        /// </summary>
        public BillingType BillingType { get; set; }

        /// <summary>
        /// Comment of the ifaRequest (just the first letters, full comment only in detail view)
        /// </summary>
        public string Comment { get; set; }

        public string CurrentDetailedResultString { get; set; }

        public string CurrentOfficialResultRemark { get; set; }

        /// <summary>
        /// Contains the current result in a human-readable form
        /// </summary>
        public string CurrentResult => ResultId == Guid.Empty || string.IsNullOrEmpty(CurrentDetailedResultString) ? string.Empty : CurrentDetailedResultString;

        /// <summary>
        /// Dilutions
        /// </summary>
        public IList<Dilution> Dilutions { get; set; }

        /// <summary>
        /// Due date of ifaRequest (latest date of reporting)
        /// </summary>
        public DateTime? DueDate { get; set; }

        public bool IsCanceled { get; set; }

        /// <summary>
        /// IsExported status
        /// </summary>
        public bool IsExported { get; set; }

        /// <summary>
        /// IsRepeater
        /// </summary>
        public bool IsRepeated { get; set; }

        public bool IsSampleArchived { get; set; }

        public string LifelongPatientId { get; set; }
        public string OrderableRunbasedLocation { get; set; }

        /// <summary>
        /// Accession number of the order
        /// </summary>
        public string OrderNumber { get; set; }

        /// <summary>
        /// Accession state of the order
        /// </summary>
        public OrderState OrderState { get; set; }

        public DateTime? PatientBirthdate { get; set; }

        /// <summary>
        /// Aggregated information about the patient: Surname, Name (age)
        /// </summary>
        public string PatientFirstName { get; set; }

        public Guid PatientId { get; set; }

        public string PatientLastName { get; set; }

        public string PreviousOfficialResultRemark { get; set; }

        /// <summary>
        /// Qualitative result (if available)
        /// </summary>
        public QualitativeRank? PreviousQualitativeRank { get; set; }

        /// <summary>
        /// Quantitative result (if available)
        /// </summary>
        public string PreviousQuantitativeResult { get; set; }

        /// <summary>
        /// Contains the previous result (the same assay for the same patient - if existant) in a human-readable form
        /// </summary>
        public string PreviousResult { get; set; }

        /// <summary>
        /// Contains the id of the sample from the previous result
        /// </summary>
        public Guid PreviousResultId { get; set; }

        public string PreviousResultVerificationApprovedBy { get; set; }

        public DateTime PreviousResultVerificationApprovedOn { get; set; }

        public Guid PreviousResultVerificationId { get; set; }

        public RequestPriority Priority { get; set; }

        /// <summary>
        /// Qualitative result (if available)
        /// </summary>
        public QualitativeRank? QualitativeRank { get; set; }

        /// <summary>
        /// Quantitative result (if available)
        /// </summary>
        public string QuantitativeResult { get; set; }

        public RepeaterType RepeaterType { get; set; }

        public DateTime RequestCreatedOn { get; set; }

        public Guid RequestId { get; set; }

        /// <summary>
        /// State of the ifaRequest (e.g. open, in progress, ...)
        /// </summary>
        public RequestState RequestState { get; set; }

        public Guid ResultId { get; set; }

        public ResultState ResultState
        {
            get
            {
                if (IsExported)
                    return ResultState.Exported;
                if (ResultVerificationState == ApprovalState.Accepted)
                    return ResultState.Verified;

                return ResultState.Calculated;
            }
        }


        public string ResultVerificationApprovedBy { get; set; }
        public DateTime ResultVerificationApprovedOn { get; set; }
        public string ResultVerificationComment { get; set; }

        public Guid ResultVerificationId { get; set; }
        public ApprovalState ResultVerificationState { get; set; }

        /// <summary>
        /// Barcode of the sample under investigation
        /// </summary>
        public string SampleBarcode { get; set; }

        /// <summary>
        /// Type of the sample
        /// </summary>
        public SampleCategory SampleCategory { get; set; }

        public Guid SampleId { get; set; }

        public string SampleLocation { get; set; }

        public string SampleTypeDisplayName { get; set; }

        public ReportScope Scope { get; set; }

        /// <summary>
        /// Sex of the patient
        /// </summary>
        public Sex Sex { get; set; }

        /// <summary>
        /// Technique of request
        /// </summary>
        public Technique Technique { get; set; }

        public string ArticleLotNumber { get; set; }
        public string ConjugateLotNumber { get; set; }
        public string BufferLotNumber { get; set; }

    }

    public enum ResultState
    {
        Calculated,
        Validated,
        Verified,
        Exported
    }

    public enum BillingType
    {
        Unknown,
        Insurance,
        Private,
        Other,
        Sender
    }

    public sealed class Dilution
    {
        public string Buffer { get; set; }

        public decimal Factor { get; set; }

        public override string ToString()
        {
            return Factor.ToString("G29");
        }
    }

    public enum OrderState
    {
        New, // all ifaRequests are open or imported
        InProgress, // at least one ifaRequest is InProcess
        PartiallyDone, // at least one ifaRequest has a result
        Done // all ifaRequests have results
    }

    public enum QualitativeRank
    {
        Unknown,
        Positive,
        Negative,
        Pathological,
        NonPathological,
        Borderline,
        WithoutResult,
        Pending,
        SeeAttachment,
        TextOnly,
        Details,
        Doppel,
        Deferred
    }

    public enum RequestPriority
    {
        Normal,
        High
    }

    public enum RepeaterType
    {
        None,
        Repeater,
        Additional
    }

    public enum RequestState
    {
        Imported,
        Open,
        InProcess,
        Processed
    }

    public enum ApprovalState
    {
        NotAccepted,
        Accepted
    }

    public enum SampleCategory
    {
        Unknown,
        Blood,
        Liquor,
        Serum,
        Tissue,
        Saliva,
        UserDefined
    }

    public enum ReportScope
    {
        Extern,
        Intern
    }

    public enum Sex
    {
        Unknown,
        Male,
        Female
    }

    public enum Technique
    {
        Unknown,
        Ifa,
        Blot,
        Elisa,
        Csq,
        Nephelometry
    }
}