int concurrent = 3;
Publisher.TransactionSimulation[] simulator = new Publisher.TransactionSimulation[concurrent];
for (var i = 0; i < concurrent; i++)
{
    int camNumber = i + 1;
    simulator[i] = new Publisher.TransactionSimulation(camNumber);
}
Parallel.ForEach(simulator, cam => cam.Start());

Task.Run(() => Thread.Sleep(Timeout.Infinite)).Wait();