import axiosClient from "../../services/axiosClient";
import type { ApiResponse } from "../../types/common";
import type {
  AddTicketCommentRequest,
  AssignTicketRequest,
  ChangeTicketStatusRequest,
  CreateTicketRequest,
  GetTicketsParams,
  PagedResult,
  Ticket,
  TicketComment,
  UpdateTicketCommentRequest,
  UpdateTicketRequest,
} from "./ticketTypes";

const BACKEND_URL = import.meta.env.VITE_API_BASE_URL;
const TICKET_URL = `${BACKEND_URL}/tickets`;

export const ticketApi = {
  getTickets(
    params: GetTicketsParams,
  ): Promise<ApiResponse<PagedResult<Ticket>>> {
    console.log(TICKET_URL);
    return axiosClient.get<
      ApiResponse<PagedResult<Ticket>>,
      ApiResponse<PagedResult<Ticket>>
    >(TICKET_URL, {
      params,
    });
  },

  getTicketById(id: string): Promise<ApiResponse<Ticket>> {
    return axiosClient.get<ApiResponse<Ticket>, ApiResponse<Ticket>>(
      `${TICKET_URL}/${id}`,
    );
  },

  createTicket(payload: CreateTicketRequest): Promise<ApiResponse<string>> {
    return axiosClient.post<
      ApiResponse<string>,
      ApiResponse<string>,
      CreateTicketRequest
    >(TICKET_URL, payload);
  },

  updateTicket(
    id: string,
    payload: UpdateTicketRequest,
  ): Promise<ApiResponse<null>> {
    return axiosClient.put<
      ApiResponse<null>,
      ApiResponse<null>,
      UpdateTicketRequest
    >(`${TICKET_URL}/${id}`, payload);
  },

  assignTicket(
    id: string,
    payload: AssignTicketRequest,
  ): Promise<ApiResponse<null>> {
    return axiosClient.patch<
      ApiResponse<null>,
      ApiResponse<null>,
      AssignTicketRequest
    >(`${TICKET_URL}/${id}/assign`, payload);
  },

  changeTicketStatus(
    id: string,
    payload: ChangeTicketStatusRequest,
  ): Promise<ApiResponse<null>> {
    return axiosClient.patch<
      ApiResponse<null>,
      ApiResponse<null>,
      ChangeTicketStatusRequest
    >(`${TICKET_URL}/${id}/status`, payload);
  },

  getComments(ticketId: string): Promise<ApiResponse<TicketComment[]>> {
    return axiosClient.get<
      ApiResponse<TicketComment[]>,
      ApiResponse<TicketComment[]>
    >(`${TICKET_URL}/${ticketId}/comments`);
  },

  addComment(
    ticketId: string,
    payload: AddTicketCommentRequest,
  ): Promise<ApiResponse<TicketComment>> {
    return axiosClient.post<
      ApiResponse<TicketComment>,
      ApiResponse<TicketComment>,
      AddTicketCommentRequest
    >(`${TICKET_URL}/${ticketId}/comments`, payload);
  },

  updateComment(
    ticketId: string,
    commentId: string,
    payload: UpdateTicketCommentRequest,
  ): Promise<ApiResponse<TicketComment>> {
    return axiosClient.put<
      ApiResponse<TicketComment>,
      ApiResponse<TicketComment>,
      UpdateTicketCommentRequest
    >(`${TICKET_URL}/${ticketId}/comments/${commentId}`, payload);
  },

  deleteComment(
    ticketId: string,
    commentId: string,
  ): Promise<ApiResponse<null>> {
    return axiosClient.delete<ApiResponse<null>, ApiResponse<null>>(
      `${TICKET_URL}/${ticketId}/comments/${commentId}`,
    );
  },
};
