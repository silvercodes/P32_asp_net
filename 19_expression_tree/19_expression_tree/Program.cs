using System;
using System.Diagnostics;
using System.Linq.Expressions;

//Func<int, int, int> delSum = (a, b) => a + b;
//Console.WriteLine(delSum(5, 3));

//Expression<Func<int, int, int>> exprSum = (a, b) => a + b;
//// exprSum(3, 4);      // ERROR
///




//Expression<Func<int, int, int>> expr = (x, y) => (x + y) * 2;

//foreach(var param in expr.Parameters)
//    Console.WriteLine($"Parameter: {param.Name}, Type: {param.Type}");

//var body = expr.Body as BinaryExpression;
//Console.WriteLine($"Body NodeType: {body.NodeType}");

//var leftPart = body.Left as BinaryExpression;
//Console.WriteLine($"Left part NodeType: {leftPart.NodeType}");

//var rightPart = body.Right as ConstantExpression;
//Console.WriteLine($"Right part NodeType: {rightPart.NodeType}, Value: {rightPart.Value}");

//var leftParamX = leftPart.Left as ParameterExpression;
//var rightParamY = leftPart.Right as ParameterExpression;
//Console.WriteLine($"Operands of Add: {leftParamX.Name} {rightParamY.Name}");





//ParameterExpression paramA = Expression.Parameter(typeof(int), "a");
//ParameterExpression paramB = Expression.Parameter(typeof(int), "b");

//BinaryExpression addBody = Expression.Add(paramA, paramB);

//Expression<Func<int, int, int>> lambdaExpr =
//    Expression.Lambda<Func<int, int, int>>(addBody, paramA, paramB);

//Func<int, int, int> compiledLambda = lambdaExpr.Compile();
//var result = compiledLambda.Invoke(3, 4);
//Console.WriteLine(result);


// ======= Сценарии использования ==========

// 1. Преобразование в другой язык (SQL, GraphQL......)

// var adultUsers = db.Users.Where(u => u.Age >= 18).ToList();
// u => u.Age >= 18  ======>   Expression<Func<User, bool>>
// Tree =====> SELECT * FROM users WHERE Age >= 18;





// 2. Динамическая генерация кода IL

//Action<TObj, TVal> CreatePropertySetter<TObj, TVal>(string propertyName)
//{
//    ParameterExpression objParam = Expression.Parameter(typeof(TObj), "obj");
//    ParameterExpression valueParam = Expression.Parameter(typeof(TVal), "value");

//    MemberExpression property = Expression.Property(objParam, propertyName);

//    BinaryExpression assignment = Expression.Assign(property, valueParam);

//    var lambda = Expression.Lambda<Action<TObj, TVal>>(assignment, objParam, valueParam);

//    return lambda.Compile();
//}

//var user = new User();
//var nameSetter = CreatePropertySetter<User, string>("Name");
//nameSetter(user, "VASIA");

//Console.WriteLine(user.Name);

//class User
//{
//    public string Name { get; set; }
//}





// 3. Построение сложных предикатов динамически

//IEnumerable<Product> productsList = new List<Product>()
//{
//    new Product {Title = "title_1", Price = 123, CategoryId = 1},
//    new Product {Title = "title_8", Price = 80, CategoryId = 2},
//    new Product {Title = "title_3", Price = 156, CategoryId = 1},
//    new Product {Title = "title_7", Price = 17, CategoryId = 1},
//};

//IQueryable<Product> products = productsList.AsQueryable();

//bool userSelectMinPrice = false;
//decimal minPrice = 100;
//bool userSelectCategory = true;
//int categoryId = 1;


//Expression<Func<Product, bool>> predicate = p => true;

//if (userSelectMinPrice)
//{
//    Expression<Func<Product, bool>> minPriceExpr = p => p.Price >= minPrice;
//    predicate = CombineAnd(predicate, minPriceExpr);
//}

//if (userSelectCategory)
//{
//    Expression<Func<Product, bool>> categoryExpr = p => p.CategoryId == categoryId;
//    predicate = CombineAnd(predicate, categoryExpr);
//}

//Expression<Func<T, bool>> CombineAnd<T>(
//    Expression<Func<T, bool>> expr1, 
//    Expression<Func<T, bool>> expr2
//)
//{
//    var parameter = Expression.Parameter(typeof(T));

//    var body = Expression.AndAlso(
//        Expression.Invoke(expr1, parameter),
//        Expression.Invoke(expr2, parameter)
//    );

//    return Expression.Lambda<Func<T, bool>>(body, parameter);
//}

//var filteredProducts = products.Where(predicate).ToList();
//filteredProducts.ForEach(p => Console.WriteLine(p.Title));


//class Product
//{
//    public string Title { get; set; }
//    public decimal Price { get; set; }
//    public int CategoryId { get; set; }
//}



// --------------------------------------------------------------------------


//List<Product> productsList = new List<Product>()
//{
//    new Product {Title = "title_1", Price = 123, CategoryId = 1},
//    new Product {Title = "title_8", Price = 80, CategoryId = 2},
//    new Product {Title = "title_3", Price = 156, CategoryId = 1},
//    new Product {Title = "title_7", Price = 17, CategoryId = 1},
//};

//IEnumerable<Product> result = productsList
//    .Where(p => p.Price >= 100)
//    .Where(p => p.Title.Contains("3"));


//IQueryable<Product> queryProducts = productsList.AsQueryable();
//IQueryable<Product> queryProductsWithTree = queryProducts
//    .Where(p => p.Price >= 100)
//    .Where(p => p.Title.Contains("3"));
//List<Product> products = queryProductsWithTree.ToList();
//int productsCount = products.Count();
//products.ForEach(p => Console.WriteLine(p.Title));
//class Product
//{
//    public string Title { get; set; }
//    public decimal Price { get; set; }
//    public int CategoryId { get; set; }
//}





class Program
{
    static void Main()
    {
        var data = new List<int>(Enumerable.Range(1, 1000000));

        var sw1 = Stopwatch.StartNew();

        IEnumerable<int> result = data
            .Where(x => x % 2 == 0)
            .Where(x => x > 500000)
            .Where(x => x % 4 == 0)
            .ToList();
        sw1.Stop();
        Console.WriteLine($"IEnumerable: {sw1.ElapsedMilliseconds}ms");

        var sw2 = Stopwatch.StartNew();

        var query = data.AsQueryable()
            .Where(x => x % 2 == 0)
            .Where(x => x > 500000)
            .Where(x => x % 4 == 0);

        Console.WriteLine($"Query: {query.Expression}");

        var result2 = query.ToList();
    
        sw2.Stop();
        Console.WriteLine($"IQueryable: {sw2.ElapsedMilliseconds}ms");
    }

    static bool IsEven(int num)
    {
        // Thread.Sleep(0);
        return num % 2 == 0;
    }
}








