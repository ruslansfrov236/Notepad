using notepad.app.Abstract;
using notepad.app.Context;
using notepad.entity;

namespace notepad.app.Concrete;

public class VerificationCodeReadRepository:ReadRepository<VerificationCode> , IVerificationReadRepository
{
    public VerificationCodeReadRepository(AppDbContext context) : base(context)
    {
    }
}