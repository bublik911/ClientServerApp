namespace ClassLib
{
    public class User
    {
        public string Name { get; }

        public int Age { get; set; }

        public User(string name, int age)
        {
            Name = name;
            Age = age;
        }
    }
}