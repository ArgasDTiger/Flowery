import axios from "axios";
import { env } from "@config/env";
import { toast } from "sonner";
import { ProblemDetails } from "@lib/problem-details";

export const api = axios.create({
  baseURL: env.API_URL,
  headers: {
    "Content-Type": "application/json",
  },
  withCredentials: true,
});

api.interceptors.response.use(
  (response) => {
    return response.data;
  },
  (error) => {
    const data: ProblemDetails = error.response?.data;
    const toastMessage = data?.title || data?.detail || error.message;
    toast.error(toastMessage);
    return Promise.reject(error);
  },
);
