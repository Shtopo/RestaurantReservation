using RestaurantReservation.API.RestaurantReservationBLL.Abstractions;

namespace RestaurantReservation.API.RestaurantReservationBLL.Services
{
    public class EmailService: IEmailService
    {
        public Task SendEmailAsync(string to, string subject, string body)
        {
            // ЗАГЛУШКА: У реальному проєкті тут буде код надсилання email.
            // Зараз ми просто виводимо інформацію в консоль для налагодження.
            Console.WriteLine("--- Відправка Email ---");
            Console.WriteLine($"Кому: {to}");
            Console.WriteLine($"Тема: {subject}");
            Console.WriteLine($"Тіло: {body}");
            Console.WriteLine("----------------------");

            return Task.CompletedTask;
        }
    }
}
