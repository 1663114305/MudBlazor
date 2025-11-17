using BlazorApp1.Models;

namespace BlazorApp1.Data;

public interface IStudentRepository
{
    List<Student> List();
    Student Get(int id);
    bool Add(Student student);
    bool Update(Student student);
    bool Delete(int id);
}