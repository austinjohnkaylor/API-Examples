# Hexagonal Architecture Pattern
Based on the [CodeMaze Article](https://code-maze.com/csharp-hexagonal-architectural-pattern/) about the Hexagonal Architecture Pattern
## Overview
`TODO`
## Components
`TODO`
## Pros and Cons
`TODO`

## Example Domain - Public Library
[Inspiration for this example domain](https://github.com/ddd-by-examples/library)
### High-level Business Process 
- A `public library` allows `patrons` to place `books` on `hold` at its various `library branches`. 
- Available `books` can be placed on `hold` only by 1 `patron` at any given point in time. 
- `Books` are either **circulating** or **restricted**, and can have **retrieval** or **usage fees**. 
- A `restricted book` can only be held by a `researcher patron`. 
- A `regular patron` is limited to 5 `holds` at any given moment, while a `researcher patron` is allowed an unlimited number of `holds`. 
- An `open-ended book hold` is active until the `patron` checks out the `book`, at which time it is completed. 
- A `closed-ended book hold` that is not completed within a fixed number of days after it was requested will expire. 
- This check is done at the beginning of a day by taking a look at `daily sheet` with `expiring holds`. 
- Only a `researcher patron` can request an `open-ended hold duration`. 
- Any `patron` with more than two overdue `checkouts` at a `library branch` will get a rejection if trying a `hold` at that same `library branch`. 
- A `book` can be checked out for up to 60 days. 
- Check for overdue checkouts is done by taking a look at `daily sheet` with overdue checkouts. 
- `Patron` interacts with his/her current holds, checkouts, etc. by taking a look at `patron profile`. 
- `Patron profile` looks like a `daily sheet`, but the information there is limited to one patron and is not necessarily daily. 
- Currently a `patron` can see current `holds` (not canceled nor expired) and current `checkouts` (including overdue). 
- Also, he/she is able to `hold` a `book` and cancel a `hold`. 
- How does a `patron` know which `books` are available to lend? 
  - `Library` has its `catalogue` of `books` where books are added together with their specific instances. 
- A specific `book instance` of a book can be added only if there is `book` with matching ISBN already in the `catalogue`. 
- Book must have non-empty title and price. 
- At the time of adding an `instance` we decide whether it will be **Circulating** or **Restricted**. 
- This enables us to have `book` with same ISBN as **circulated** and **restricted** at the same time (for instance, there is a book signed by the author that we want to keep as Restricted)

### High-Level Definitions
#### Public Library
- Allows patrons to place books on hold at it's various library branches
#### Book
- Metadata about the book
  - Title
  - Author
  - ISBN
  - Price
- Are either **circulating** or **restricted** 
- Can have **retrieval fees** or **usage fees**
- Can be **restricted**
  - restricted books can only be held by researcher patrons
- Can be put on hold by only one patron at a time
#### Patron
- **Regular Patron**
  - limited to 5 holds at a time 
- **Researcher Patron**
  - unlimited number of holds
#### Library Branches
- Have a catalog of book instances
#### Holds
- **Opened-ended book hold**
  - active until the patron checks out the book, which completes the hold
  - only a researcher patron may request this kind of hold
- **Closed-ended book hold**
  - active until the reservation period expires after a fixed number of days
#### Daily Sheet
- Lists expiring closed-ended book holds
- Lists current open-ended book holds
- Lists overdue Checkouts
#### Checkouts
- Any patron with more than 2 overdue checkouts at a given library branch will get a rejection if they try to place a hold at that same branch. Placing a hold at another branch is allowed.
- A book can be checked out for up to 60 days
- Overdue checkouts are listed on the Daily Sheet
#### Patron Profile
- Profile for a given Library Patron
- Lists current holds at branches, but not cancelled or expired
- Lists type of patron
- Lists current checkouts at specified branches, including overdue checkouts
- Looks like the Daily Sheet, but is limited to the Patron and is not scoped to a given day
- Can cancel a hold here
- Can place a hold here
#### Catalogue
- Patron knows which books are available to lend from the catalogue
- Differs per library branch
- Made up of books that belong to a specific library
- The same book could be at different library branches
- The same book could have multiple copies at one library branch
#### Book Instance
- Physical copy of a book
- Can only be added to a catalogue if there is a book with matching ISBN already in the catalog
- When an instance is added to a libraries catalogue, its decided whether it is circulating or restricted
- This enables us to have `book` with same ISBN as **circulated** and **restricted** at the same time (for instance, there is a book signed by the author that we want to keep as Restricted)

### Questions to answer from High-Level Definitions
- What happens when a `regular patron` tries to initiate an `open-ended hold`?
- 

### [High-Level Processes](https://github.com/ddd-by-examples/library/blob/master/docs/big-picture.md)
### [Mid-Level Process Mapping](https://github.com/ddd-by-examples/library/blob/master/docs/example-mapping.md)
### [Low-Level Process Mapping]()
### Bounded-Contexts
#### Lending
- Containing all the business processes related to book lending, book checkout, book holding, and book return
#### Catalog
- Books
- Book Instances at Library Branches

[Back to solution README.md](../README.md)

