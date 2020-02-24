namespace EStore.Models.Order
{
    public enum OrderStatus
    {
        Canceled = 0,
        Closed = 1,
        Completed = 2,
        SuspectedFraud = 3,
        OnHold = 4,
        PaymentReview = 5,
        Pending = 6,
        PendingPayment = 7,
        Processing = 8,
        Submitted = 9
    }
}
