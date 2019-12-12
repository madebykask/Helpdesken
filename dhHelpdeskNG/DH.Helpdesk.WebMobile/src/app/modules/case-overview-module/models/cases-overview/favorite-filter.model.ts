export class FavoriteFilterModel {
  id: string;
  name: string;
  fields: { [key: string]: string };
}

export class CustomerFavoriteFilterModel {
  customerId: number;
  customerName: string;
  favorites: FavoriteFilterModel[];
}

