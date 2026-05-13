import { createAsyncThunk } from "@reduxjs/toolkit";
import type { AppError } from "../../types/common";
import { toAppError } from "../../services/errorHandler";
import { ticketApi } from "./ticketApi";
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

export const getTicketsThunk = createAsyncThunk<
  PagedResult<Ticket>,
  GetTicketsParams | undefined,
  { rejectValue: AppError }
>("tickets/getTickets", async (params, { rejectWithValue }) => {
  try {
    const response = await ticketApi.getTickets(params ?? {});
    console.log("getTicketsThunk response:", response);
    return response.data;
  } catch (error) {
    return rejectWithValue(toAppError(error));
  }
});

export const getTicketByIdThunk = createAsyncThunk<
  Ticket,
  string,
  { rejectValue: AppError }
>("tickets/getTicketById", async (id, { rejectWithValue }) => {
  try {
    const response = await ticketApi.getTicketById(id);
    return response.data;
  } catch (error) {
    return rejectWithValue(toAppError(error));
  }
});

export const createTicketThunk = createAsyncThunk<
  string,
  CreateTicketRequest,
  { rejectValue: AppError }
>("tickets/createTicket", async (payload, { rejectWithValue }) => {
  try {
    const response = await ticketApi.createTicket(payload);
    return response.data;
  } catch (error) {
    return rejectWithValue(toAppError(error));
  }
});

export const updateTicketThunk = createAsyncThunk<
  void,
  {
    id: string;
    payload: UpdateTicketRequest;
  },
  { rejectValue: AppError }
>("tickets/updateTicket", async ({ id, payload }, { rejectWithValue }) => {
  try {
    await ticketApi.updateTicket(id, payload);
  } catch (error) {
    return rejectWithValue(toAppError(error));
  }
});

export const assignTicketThunk = createAsyncThunk<
  void,
  {
    id: string;
    payload: AssignTicketRequest;
  },
  { rejectValue: AppError }
>("tickets/assignTicket", async ({ id, payload }, { rejectWithValue }) => {
  try {
    await ticketApi.assignTicket(id, payload);
  } catch (error) {
    return rejectWithValue(toAppError(error));
  }
});

export const changeTicketStatusThunk = createAsyncThunk<
  void,
  {
    id: string;
    payload: ChangeTicketStatusRequest;
  },
  { rejectValue: AppError }
>(
  "tickets/changeTicketStatus",
  async ({ id, payload }, { rejectWithValue }) => {
    try {
      await ticketApi.changeTicketStatus(id, payload);
    } catch (error) {
      return rejectWithValue(toAppError(error));
    }
  },
);

export const getTicketCommentsThunk = createAsyncThunk<
  TicketComment[],
  string,
  { rejectValue: AppError }
>("tickets/getTicketComments", async (ticketId, { rejectWithValue }) => {
  try {
    const response = await ticketApi.getComments(ticketId);
    return response.data;
  } catch (error) {
    return rejectWithValue(toAppError(error));
  }
});

export const addTicketCommentThunk = createAsyncThunk<
  TicketComment,
  {
    ticketId: string;
    payload: AddTicketCommentRequest;
  },
  { rejectValue: AppError }
>(
  "tickets/addTicketComment",
  async ({ ticketId, payload }, { rejectWithValue }) => {
    try {
      const response = await ticketApi.addComment(ticketId, payload);
      return response.data;
    } catch (error) {
      return rejectWithValue(toAppError(error));
    }
  },
);

export const updateTicketCommentThunk = createAsyncThunk<
  TicketComment,
  {
    ticketId: string;
    commentId: string;
    payload: UpdateTicketCommentRequest;
  },
  { rejectValue: AppError }
>(
  "tickets/updateTicketComment",
  async ({ ticketId, commentId, payload }, { rejectWithValue }) => {
    try {
      const response = await ticketApi.updateComment(
        ticketId,
        commentId,
        payload,
      );

      return response.data;
    } catch (error) {
      return rejectWithValue(toAppError(error));
    }
  },
);

export const deleteTicketCommentThunk = createAsyncThunk<
  string,
  {
    ticketId: string;
    commentId: string;
  },
  { rejectValue: AppError }
>(
  "tickets/deleteTicketComment",
  async ({ ticketId, commentId }, { rejectWithValue }) => {
    try {
      await ticketApi.deleteComment(ticketId, commentId);
      return commentId;
    } catch (error) {
      return rejectWithValue(toAppError(error));
    }
  },
);
