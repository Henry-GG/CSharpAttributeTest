using System;
using System.Reflection;


public class StringLengthAttribute : Attribute
{
    public StringLengthAttribute(string displayName,int maxLength,int minLength,string errorMsg)
    {
        this.DisplayName = displayName;
        this.MaxLength = maxLength;
        this.MinLength = minLength;
        this.ErrorMessage = errorMsg;
    }

    public string DisplayName { get; private set; }
    public int MaxLength { get; private set; }
    public string ErrorMessage { get; private set; }
    public int MinLength { get; private set; }
}

public class Order
{
    [StringLength("订单号",6,3,"{0}的长度不在{1}和{2}之间")]
    public string OrderID { get; set;   }
}
class Program
{
    #region 使用特性类的过程

    //验证过程
    //1.通过映射，找到成员属性关联的特性类实例，
    //2.使用特性类实例对新对象的数据进行验证

    //用特性类验证订单号长度
    public static bool isIDLengthValid(int IDLength, MemberInfo member)
    {
        foreach (object attribute in member.GetCustomAttributes(true)) //2.通过映射，找到成员属性上关联的特性类实例，
        {
            if (attribute is StringLengthAttribute)//3.如果找到了限定长度的特性类对象，就用这个特性类对象验证该成员
            {
                StringLengthAttribute attr = (StringLengthAttribute)attribute;
                if (IDLength < attr.MinLength || IDLength > attr.MaxLength)
                {
                    string displayName = attr.DisplayName;
                    int maxLength = attr.MaxLength;
                    int minLength = attr.MinLength;
                    string error = attr.ErrorMessage;
                    Console.WriteLine(error, displayName, maxLength, minLength);//验证失败，提示错误
                    return false;
                }
                else return true;
            }

        }
        return false;
    }


    //验证订单对象是否规范
    public static bool IsOrderValid(Order order)
    {
        if (order == null) return false;
        foreach (PropertyInfo p in typeof(Order).GetProperties())
        {
            if (isIDLengthValid(order.OrderID.Length, p))//1记录下新对象需要验证的数据，
            { return true; }
        }
        return false;

    }
    #endregion
    public static void Main()
    {
        Order order = new Order();
        do
        {
            Console.WriteLine("请输入订单号：");
            order.OrderID = Console.ReadLine();
        }
        while (!IsOrderValid(order));
        Console.WriteLine("订单号输入正确，按任意键退出！");
        Console.ReadKey();
    }

}
