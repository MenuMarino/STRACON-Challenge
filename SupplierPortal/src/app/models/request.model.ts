export interface Supplier {
  supplierId: number;
  name: string;
  address: string;
  contact: string;
  hasPartnership: boolean;
}

export interface Product {
  productId: number;
  requestId: number;
  name: string;
  quantity: number;
  unitPrice: number;
  productUrl: string;
}

export interface Request {
  requestId: number;
  supplierId: number;
  totalPrice: number;
  status: string;
  createdBy: string;
  supplier: Supplier;
  products: Product[];
}
