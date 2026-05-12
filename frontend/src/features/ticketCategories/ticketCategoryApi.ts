import axiosClient from "../../services/axiosClient";
import type { ApiMessageResponse, ApiResponse } from "../../types/common";
import type {
  CreateTicketCategoryRequest,
  TicketCategory,
  UpdateTicketCategoryRequest,
} from "./ticketCategoryTypes";

const BACKEND_URL = import.meta.env.BACKEND_URL || "http://localhost:5049/api";
const TICKET_CATEGORY_URL = `${BACKEND_URL}/ticket-categories`;

export const ticketCategoryApi = {
  getAll(): Promise<ApiResponse<TicketCategory[]>> {
    return axiosClient.get<
      ApiResponse<TicketCategory[]>,
      ApiResponse<TicketCategory[]>
    >(TICKET_CATEGORY_URL);
  },

  getById(id: string): Promise<ApiResponse<TicketCategory>> {
    return axiosClient.get<
      ApiResponse<TicketCategory>,
      ApiResponse<TicketCategory>
    >(`${TICKET_CATEGORY_URL}/${id}`);
  },

  create(
    payload: CreateTicketCategoryRequest,
  ): Promise<ApiResponse<TicketCategory>> {
    return axiosClient.post<
      ApiResponse<TicketCategory>,
      ApiResponse<TicketCategory>,
      CreateTicketCategoryRequest
    >(TICKET_CATEGORY_URL, payload);
  },

  update(
    id: string,
    payload: UpdateTicketCategoryRequest,
  ): Promise<ApiMessageResponse> {
    return axiosClient.put<
      ApiMessageResponse,
      ApiMessageResponse,
      UpdateTicketCategoryRequest
    >(`${TICKET_CATEGORY_URL}/${id}`, payload);
  },

  delete(id: string): Promise<ApiMessageResponse> {
    return axiosClient.delete<ApiMessageResponse, ApiMessageResponse>(
      `${TICKET_CATEGORY_URL}/${id}`,
    );
  },
};
