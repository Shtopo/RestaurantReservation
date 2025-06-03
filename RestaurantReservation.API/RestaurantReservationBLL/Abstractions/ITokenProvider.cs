namespace RestaurantReservation.API.RestaurantReservationBLL.Abstractions
{
    public interface ITokenProvider
    {
        string GenerateToken(int userId, string role);
    }
}
