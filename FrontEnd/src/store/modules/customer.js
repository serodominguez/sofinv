import axios from "axios";
import { jwtDecode } from "jwt-decode";
import store from "@/store";

const state = {
  customers: [],
  selectedCustomer: null,
  loading: false,
  error: null,
};

const mutations = {
  SET_CUSTOMERS(state, customers) {
    state.customers = customers;
  },
  SET_SELECTED_CUSTOMER(state, customer) {
    state.selectedCustomer = customer;
  },
  SET_LOADING(state, loading) {
    state.loading = loading;
  },
  SET_ERROR(state, error) {
    state.error = error;
  },
};

const isExpired = (token) => {
  if (!token) return false;
  const decodedToken = jwtDecode(token);
  const currentTime = Date.now() / 1000;
  return decodedToken.exp < currentTime;
};

const actions = {
  async fetchCustomers({ commit, rootState }) {
    commit("SET_LOADING", true);
    try {
      const token = rootState.token;
      if (isExpired(token)) {
        await store.dispatch("logoff");
        return;
      }

      const header = { Authorization: `Bearer ${token}` };
      let configuration = { headers: header };
      const response = await axios.get("api/Customers/ReadCustomers", configuration);
      commit("SET_CUSTOMERS", response.data);
    } catch (error) {
      commit("SET_ERROR", error.message);
    } finally {
      commit("SET_LOADING", false);
    }
  },
  async fetchCustomer({ commit, rootState }, text) {
    commit("SET_LOADING", true);
    try {
      const token = rootState.token;
      if (isExpired(token)) {
        await store.dispatch("logoff");
        return;
      }

      let header = { Authorization: "Bearer " + token };
      let configuration = { headers: header };
      const response = await axios.get(`api/Customers/SearchCustomer/${text}`, configuration);
      commit("SET_CUSTOMERS", response.data);
    } catch (error) {
      commit("SET_ERROR", error.message);
    } finally {
      commit("SET_LOADING", false);
    }
  },
  async selectCustomers({ commit, rootState }) {
    commit("SET_LOADING", true);
    try {
      const token = rootState.token;
      if (isExpired(token)) {
        await store.dispatch("logoff");
        return;
      }

      let header = { Authorization: "Bearer " + token };
      let configuration = { headers: header };
      const response = await axios.get("api/Customers/SelectCustomers", configuration);
      commit("SET_CUSTOMERS", response.data);
    } catch (error) {
      commit("SET_ERROR", error.message);
    } finally {
      commit("SET_LOADING", false);
    }
  },
  async createCustomer({ commit, rootState }, customer) {
    try {
      const token = rootState.token;
      if (isExpired(token)) {
        await store.dispatch("logoff");
        return;
      }

      let header = { Authorization: "Bearer " + token };
      let configuration = { headers: header };
      await axios.post("api/Customers/CreateCustomer", customer, configuration);
    } catch (error) {
      commit("SET_ERROR", error.response);
      throw error;
    }
  },
  async updateCustomer({ commit, rootState }, customer) {
    try {
      const token = rootState.token;
      if (isExpired(token)) {
        await store.dispatch("logoff");
        return;
      }

      let header = { Authorization: "Bearer " + token };
      let configuration = { headers: header };
      await axios.put("api/Customers/UpdateCustomer", customer, configuration);
    } catch (error) {
      commit("SET_ERROR", error.response);
      throw error;
    }
  },
  async enabledCustomer({ commit, dispatch, rootState }, id) {
    try {
      const token = rootState.token;
      if (isExpired(token)) {
        await store.dispatch("logoff");
        return;
      }

      let header = { Authorization: "Bearer " + token };
      let configuration = { headers: header };
      await axios.put(`api/Customers/EnabledCustomer/${id}`, {}, configuration);
      dispatch("fetchCustomers");
    } catch (error) {
      commit("SET_ERROR", error.response);
      throw error;
    }
  },
  async disabledCustomer({ commit, dispatch, rootState }, id) {
    try {
      const token = rootState.token;
      if (isExpired(token)) {
        await store.dispatch("logoff");
        return;
      }

      let header = { Authorization: "Bearer " + token };
      let configuration = { headers: header };
      await axios.put(`api/Customers/DisabledCustomer/${id}`, {}, configuration);
      dispatch("fetchCustomers");
    } catch (error) {
      commit("SET_ERROR", error.response);
      throw error;
    }
  },
};

const getters = {
  customers: (state) => state.customers,
  selectedCustomer: (state) => state.selectedCustomer,
  loading: (state) => state.loading,
  error: (state) => state.error,
};

export default {
  namespaced: true,
  state,
  mutations,
  actions,
  getters,
};