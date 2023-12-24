using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ChatCore;

public class Users
{
    public Users(string name)
    {
        Name = name;    
    }
    public string Name { get; set; }
    public override string ToString() => Name;
    public override bool Equals(object? obj)
    {
        if(obj == null || GetType() != obj.GetType())
        {
            return false;
        }
        var user = obj as Users;    
        return Name.Equals(user?.Name); 
    }
    public override int GetHashCode() => Name.GetHashCode();
    public static Users Unknown() => new Users("[Unknown]");
}
public class UserComparer : IEqualityComparer<Users>
{
    public bool Equals(Users? x, Users? y)
    {
        return x?.Equals(y) ?? false;
    }
    public int GetHashCode([DisallowNull] Users obj)
    {
        return obj.GetHashCode();
    }    
}
