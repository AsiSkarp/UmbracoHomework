import pyodbc
import random
from datetime import datetime

# Database connection
connection_string = (
    "Driver={SQL Server};"
    "Server=LAPTOP-AVRO3B2T\\SQLEXPRESS;"
    "Database=AcmeCorpDb;"
    "Trusted_Connection=yes;"
)

customers = [
    {
        "FirstName": "Grace",
        "LastName": "Hopper",
        "Email": "grace@hopper.com",
        "DateOfBirth": "1906-12-09",
    },
    {
        "FirstName": "John",
        "LastName": "von Neumann",
        "Email": "john@neumann.com",
        "DateOfBirth": "1903-12-28",
    },
    {
        "FirstName": "Tim",
        "LastName": "Berners-Lee",
        "Email": "tim@berners-lee.com",
        "DateOfBirth": "1955-06-08",
    },
    {
        "FirstName": "Ada",
        "LastName": "Lovelace",
        "Email": "ada@lovelace.com",
        "DateOfBirth": "1815-12-10",
    },
    {
        "FirstName": "Donald",
        "LastName": "Knuth",
        "Email": "donald@knuth.com",
        "DateOfBirth": "1938-01-10",
    },
    {
        "FirstName": "Edsger",
        "LastName": "Dijkstra",
        "Email": "edsger@dijkstra.com",
        "DateOfBirth": "1930-05-11",
    },
    {
        "FirstName": "Marvin",
        "LastName": "Minsky",
        "Email": "marvin@minsky.com",
        "DateOfBirth": "1927-08-09",
    },
    {
        "FirstName": "Linus",
        "LastName": "Torvalds",
        "Email": "linus@torvalds.com",
        "DateOfBirth": "1969-12-28",
    },
    {
        "FirstName": "James",
        "LastName": "Gosling",
        "Email": "james@gosling.com",
        "DateOfBirth": "1955-05-19",
    },
    {
        "FirstName": "Margaret",
        "LastName": "Hamilton",
        "Email": "margaret@hamilton.com",
        "DateOfBirth": "1936-08-17",
    },
    {
        "FirstName": "Katherine",
        "LastName": "Johnson",
        "Email": "katherine@johnson.com",
        "DateOfBirth": "1918-08-26",
    },
    {
        "FirstName": "Anita",
        "LastName": "Borg",
        "Email": "anita@borg.com",
        "DateOfBirth": "1949-01-17",
    },
    {
        "FirstName": "Steve",
        "LastName": "Jobs",
        "Email": "steve@jobs.com",
        "DateOfBirth": "1955-02-24",
    },
    {
        "FirstName": "Larry",
        "LastName": "Page",
        "Email": "larry@page.com",
        "DateOfBirth": "1973-03-26",
    },
    {
        "FirstName": "Sergey",
        "LastName": "Brin",
        "Email": "sergey@brin.com",
        "DateOfBirth": "1973-08-21",
    },
    {
        "FirstName": "Guido",
        "LastName": "van Rossum",
        "Email": "guido@rossum.com",
        "DateOfBirth": "1956-01-31",
    },
    {
        "FirstName": "Bjarne",
        "LastName": "Stroustrup",
        "Email": "bjarne@stroustrup.com",
        "DateOfBirth": "1950-12-30",
    },
    {
        "FirstName": "Whitfield",
        "LastName": "Diffie",
        "Email": "whitfield@diffie.com",
        "DateOfBirth": "1944-06-05",
    },
    {
        "FirstName": "Martin",
        "LastName": "Hellman",
        "Email": "martin@hellman.com",
        "DateOfBirth": "1945-10-02",
    },
    {
        "FirstName": "Shafi",
        "LastName": "Goldwasser",
        "Email": "shafi@goldwasser.com",
        "DateOfBirth": "1958-11-25",
    },
    {
        "FirstName": "Silvio",
        "LastName": "Micali",
        "Email": "silvio@micali.com",
        "DateOfBirth": "1954-10-13",
    },
    {
        "FirstName": "Mark",
        "LastName": "Dean",
        "Email": "mark@dean.com",
        "DateOfBirth": "1957-03-02",
    },
    {
        "FirstName": "Frances",
        "LastName": "Allen",
        "Email": "frances@allen.com",
        "DateOfBirth": "1932-08-04",
    },
    {
        "FirstName": "Guo",
        "LastName": "Moruo",
        "Email": "guo@moruo.com",
        "DateOfBirth": "1892-11-16",
    },
    {
        "FirstName": "Victor",
        "LastName": "Bahl",
        "Email": "victor@bahl.com",
        "DateOfBirth": "1963-01-01",
    },
    {
        "FirstName": "Bill",
        "LastName": "Gates",
        "Email": "bill@gates.com",
        "DateOfBirth": "1955-10-28",
    },
]


# Open the serial numbers text file
with open("serial_numbers.txt", "r") as file:
    serial_numbers = [line.strip() for line in file.readlines()]


# Function to insert a customer into the Customers table
def insert_customer(cursor, first_name, last_name, email, date_of_birth):
    customer_insert_query = """
    INSERT INTO Customers (FirstName, LastName, Email, DateOfBirth)
    OUTPUT INSERTED.ID
    VALUES (?, ?, ?, ?)
    """
    cursor.execute(customer_insert_query, first_name, last_name, email, date_of_birth)
    return cursor.fetchone()[0]  # Returns the inserted Customer ID


# Function to insert an entry into the Entries table
def insert_entry(cursor, serial_number, customer_id):
    entry_insert_query = """
    INSERT INTO Entries (SerialNumber, CustomerId, EntryTime)
    VALUES (?, ?, ?)
    """
    entry_time = datetime.now()
    cursor.execute(entry_insert_query, serial_number, customer_id, entry_time)


# Main execution
def main():
    try:
        # Establish connection to SQL Server
        conn = pyodbc.connect(connection_string)
        cursor = conn.cursor()

        # Insert 25 customers and their entries
        for i in range(25):
            # Get customer details from the mock data
            customer = random.choice(customers)

            # Insert customer and retrieve the ID
            customer_id = insert_customer(
                cursor,
                customer["FirstName"],
                customer["LastName"],
                customer["Email"],
                customer["DateOfBirth"],
            )

            # Get the next serial number in line
            serial_number = serial_numbers[i]

            # Insert entry for the customer
            insert_entry(cursor, serial_number, customer_id)

            # Commit each insert operation
            conn.commit()

            print(
                f"Inserted Customer ID {customer_id} with Serial Number {serial_number}"
            )

    except Exception as e:
        print(f"An error occurred: {e}")
    finally:
        # Close the connection
        cursor.close()
        conn.close()


if __name__ == "__main__":
    main()
