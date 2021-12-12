namespace Subscriber.Events;

public record struct TransactionEvent(int TransactionId, string TransactionNumber, string Name, string Amount, DateTime Timestamp);