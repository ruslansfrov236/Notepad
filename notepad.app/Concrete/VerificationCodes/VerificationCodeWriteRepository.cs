using notepad.app.Abstract;
using notepad.app.Context;
using notepad.entity;

namespace notepad.app.Concrete;

public class VerificationCodeWriteRepository:WriteRepository<VerificationCode>, IVerificationWriteRepository
{
    public VerificationCodeWriteRepository(AppDbContext context) : base(context)
    {
    }
}