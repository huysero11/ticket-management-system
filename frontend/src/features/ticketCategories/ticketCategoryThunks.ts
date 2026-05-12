import { createAsyncThunk } from "@reduxjs/toolkit";
import {
  type UpdateTicketCategoryPayload,
  type CreateTicketCategoryRequest,
  type TicketCategory,
} from "./ticketCategoryTypes";
import { ticketCategoryApi } from "./ticketCategoryApi";
import { toAppError } from "../../services/errorHandler";
import type { AppError } from "../../types/common";

export const getTicketCategoriesThunk = createAsyncThunk<
  TicketCategory[],
  void,
  { rejectValue: AppError }
>("ticketCategories/getAll", async (_, { rejectWithValue }) => {
  try {
    const res = await ticketCategoryApi.getAll();
    return res.data;
  } catch (error) {
    return rejectWithValue(toAppError(error));
  }
});

export const getTicketCategoryByIdThunk = createAsyncThunk<
  TicketCategory,
  string,
  { rejectValue: AppError }
>("ticketCategories/getById", async (id, { rejectWithValue }) => {
  try {
    const res = await ticketCategoryApi.getById(id);
    return res.data;
  } catch (error) {
    return rejectWithValue(toAppError(error));
  }
});

export const createTicketCategoryThunk = createAsyncThunk<
  TicketCategory,
  CreateTicketCategoryRequest,
  { rejectValue: AppError }
>("ticketCategories/create", async (payload, { rejectWithValue }) => {
  try {
    const res = await ticketCategoryApi.create(payload);
    return res.data;
  } catch (error) {
    return rejectWithValue(toAppError(error));
  }
});

export const updateTicketCategoryThunk = createAsyncThunk<
  void,
  UpdateTicketCategoryPayload,
  { rejectValue: AppError }
>("ticketCategories/update", async (payload, { rejectWithValue }) => {
  try {
    await ticketCategoryApi.update(payload.id, payload.request);
  } catch (error) {
    return rejectWithValue(toAppError(error));
  }
});

export const deleteTicketCategoryThunk = createAsyncThunk<
  void,
  string,
  { rejectValue: AppError }
>("ticketCategories/delete", async (id, { rejectWithValue }) => {
  try {
    await ticketCategoryApi.delete(id);
  } catch (error) {
    return rejectWithValue(toAppError(error));
  }
});
