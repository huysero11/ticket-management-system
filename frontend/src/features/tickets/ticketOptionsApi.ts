import axiosClient from "../../services/axiosClient";
import type { ApiResponse } from "../../types/common";

export interface TicketCategoryOption {
  id: string;
  name: string;
}

export interface AgentOption {
  id: string;
  fullName: string;
  email: string;
}

export const ticketOptionsApi = {
  getCategories(): Promise<ApiResponse<TicketCategoryOption[]>> {
    return axiosClient.get<
      ApiResponse<TicketCategoryOption[]>,
      ApiResponse<TicketCategoryOption[]>
    >("/ticket-categories");
  },

  getAgents(): Promise<ApiResponse<AgentOption[]>> {
    return axiosClient.get<
      ApiResponse<AgentOption[]>,
      ApiResponse<AgentOption[]>
    >("/users", {
      params: {
        role: "Agent",
      },
    });
  },
};
