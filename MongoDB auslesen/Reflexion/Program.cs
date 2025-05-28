// See https://aka.ms/new-console-template for more information

Person person1 = new Person{Id = 1, Name = "Hugo", Age = 22};

foreach (var probertyInfo in typeof(Person).GetProperties())
{
    Console.WriteLine(probertyInfo.Name + " " + probertyInfo.GetValue(person1));
}
class Person
{
    public int Id { get; set; }
    public string Name { get; set; }
    public int Age { get; set; }
}