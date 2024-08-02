# Shared Resources
This directory contains resources that are shared across multiple projects. This includes things like shared code, shared data, and shared documentation.

# Projects
## EntityFramework
This project contains the shared Entity Framework code that is used by the other projects. This includes the `DbContext` and the `DbSet` classes.
### ECommerce
This folder contains the Entity Framework classes for the ECommerce database.
### SchoolSystem
This folder contains the Entity Framework classes for the SchoolSystem database.
#### Relationships between Entities

1. **Student**
    - **Properties**: `StudentId`, `FirstName`, `LastName`, `Email`, `GradeLevel`
    - **Navigation Property**: `ICollection<Enrollment> Enrollments`
    - **Relationships**:
        - One-to-Many with `Enrollment` (One `Student` can have many `Enrollments`)

2. **Teacher**
    - **Properties**: `TeacherId`, `FirstName`, `LastName`, `Email`
    - **Navigation Property**: `ICollection<Course> Courses`
    - **Relationships**:
        - One-to-Many with `Course` (One `Teacher` can teach many `Courses`)

3. **Course**
    - **Properties**: `CourseId`, `Title`, `Credits`, `TeacherId`
    - **Navigation Properties**: `Teacher Teacher`, `ICollection<Enrollment> Enrollments`
    - **Relationships**:
        - Many-to-One with `Teacher` (Many `Courses` can be taught by one `Teacher`)
        - One-to-Many with `Enrollment` (One `Course` can have many `Enrollments`)

4. **Enrollment**
    - **Properties**: `EnrollmentId`, `CourseId`, `StudentId`
    - **Navigation Properties**: `Course Course`, `Student Student`
    - **Relationships**:
        - Many-to-One with `Course` (Many `Enrollments` can be for one `Course`)
        - Many-to-One with `Student` (Many `Enrollments` can be for one `Student`)

## Further Reading

## Main Readme
[Back to Solution README](../README.md)
