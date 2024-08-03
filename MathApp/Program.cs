var builder = WebApplication.CreateBuilder(args);
var app = builder.Build();

static int calculate(int firstNum, int secondNum, string op)
{
    try
    {
        switch (op)
        {
            case "subtract":
                return secondNum - firstNum;
            case "multiply":
                return secondNum * firstNum;
            case "divide":
                return secondNum / firstNum;
            case "mod":
                return secondNum % firstNum;
            case "add":
                return secondNum + firstNum;
            case null:
            default:
                throw new NullReferenceException("'operator' is invalid");
        }
    }
    catch (InvalidOperationException)
    {
        throw;
    }
    catch (NullReferenceException)
    {
        throw;
    }
    catch (DivideByZeroException)
    {
        throw;
    }
}

app.Run(async (HttpContext context) =>
{
    try
    {
        IQueryCollection urlParams = context.Request.Query;
        string firstNum = urlParams["firstNum"];
        string secondNum = urlParams["secondNum"];
        string op = urlParams["operator"];
        bool hasFirstNum = firstNum != null;
        bool hasSecondNum = secondNum != null;
        bool hasOp = op != null;
        if (urlParams.Count > 3)
        {
            throw new InvalidDataException("Only support 3 paramters. 2 numbers and an operator");
        }
        if (!hasFirstNum || !hasSecondNum || !hasOp)
        {
            context.Response.StatusCode = 400;
            if (!hasFirstNum)
            {
                await context.Response.WriteAsync("'firstNum' is invalid\n");
            }
            if (!hasSecondNum)  
            {
                await context.Response.WriteAsync("'secondNum' is invalid\n");
            }
            if (!hasOp)
            {
                await context.Response.WriteAsync("'operator' is invalid");
            }
        }
        else
        {
            int res = calculate(int.Parse(firstNum), int.Parse(secondNum), op);
            context.Response.StatusCode = 200;
            await context.Response.WriteAsync(Convert.ToString(res));
        }
    }
    catch (Exception ex)
    {
        context.Response.StatusCode = 400;
        await context.Response.WriteAsync(ex.Message);
    }
});

app.Run();
