export interface Student{
  id: number;
  firstName: string;
  lastName: string;
  city: string
  address:Address
}
export interface Address {
  addressId: number;       // Matches C# AddressId
  streetNumber?: string;   // Matches C# StreetNumber (optional)
  city?: string;           // Matches C# City (optional)
  state?: string;          // Matches C# State (optional)
  country?: string;        // Matches C# Country (optional)
  zipcode?: number;        // Matches C# Zipcode (optional)
}
