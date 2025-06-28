import axios from "axios";
import { jwtDecode } from "jwt-decode";
import store from "@/store";

const state = {
  suppliers: [],
  selectedSupplier: null,
  loading: false,
  error: null,
};

const mutations = {
  SET_SUPPLIERS(state, suppliers) {
    state.suppliers = suppliers;
  },
  SET_SELECTED_SUPPLIER(state, supplier) {
    state.selectedSupplier = supplier;
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
  async fetchSuppliers({ commit, rootState }) {
    commit("SET_LOADING", true);
    try {
      const token = rootState.token;
      if (isExpired(token)) {
        await store.dispatch("logoff");
        return;
      }

      let header = { Authorization: "Bearer " + token };
      let configuration = { headers: header };
      const response = await axios.get("api/Suppliers/ReadSuppliers", configuration);
      commit("SET_SUPPLIERS", response.data);
    } catch (error) {
      commit("SET_ERROR", error.message);
    } finally {
      commit("SET_LOADING", false);
    }
  },
  async fetchSupplier({ commit, rootState }, text) {
    commit("SET_LOADING", true);
    try {
      const token = rootState.token;
      if (isExpired(token)) {
        await store.dispatch("logoff");
        return;
      }

      let header = { Authorization: "Bearer " + token };
      let configuration = { headers: header };
      const response = await axios.get(`api/Suppliers/SearchSupplier/${text}`, configuration);
      commit("SET_SUPPLIERS", response.data);
    } catch (error) {
      commit("SET_ERROR", error.message);
    } finally {
      commit("SET_LOADING", false);
    }
  },
  async selectSuppliers({ commit, rootState }) {
    commit("SET_LOADING", true);
    try {
      const token = rootState.token;
      if (isExpired(token)) {
        await store.dispatch("logoff");
        return;
      }

      let header = { Authorization: "Bearer " + token };
      let configuration = { headers: header };
      const response = await axios.get("api/Suppliers/SelectSuppliers", configuration);
      commit("SET_SUPPLIERS", response.data);
    } catch (error) {
      commit("SET_ERROR", error.message);
    } finally {
      commit("SET_LOADING", false);
    }
  },
  async createSupplier({ commit, rootState }, supplier) {
    try {
      const token = rootState.token;
      if (isExpired(token)) {
        await store.dispatch("logoff");
        return;
      }

      let header = { Authorization: "Bearer " + token };
      let configuration = { headers: header };
      await axios.post("api/Suppliers/CreateSupplier", supplier, configuration);
    } catch (error) {
      commit("SET_ERROR", error.response);
      throw error;
    }
  },
  async updateSupplier({ commit, rootState }, supplier) {
    try {
      const token = rootState.token;
      if (isExpired(token)) {
        await store.dispatch("logoff");
        return;
      }

      let header = { Authorization: "Bearer " + token };
      let configuration = { headers: header };
      await axios.put("api/Suppliers/UpdateSupplier", supplier, configuration);
    } catch (error) {
      commit("SET_ERROR", error.response);
      throw error;
    }
  },
  async enabledSupplier({ commit, dispatch, rootState }, id) {
    try {
      const token = rootState.token;
      if (isExpired(token)) {
        await store.dispatch("logoff");
        return;
      }

      let header = { Authorization: "Bearer " + token };
      let configuration = { headers: header };
      await axios.put(`api/Suppliers/EnabledSupplier/${id}`, {}, configuration);
      dispatch("fetchSuppliers");
    } catch (error) {
      commit("SET_ERROR", error.response);
      throw error;
    }
  },
  async disabledSupplier({ commit, dispatch, rootState }, id) {
    try {
      const token = rootState.token;
      if (isExpired(token)) {
        await store.dispatch("logoff");
        return;
      }

      let header = { Authorization: "Bearer " + token };
      let configuration = { headers: header };
      await axios.put(`api/Suppliers/DisabledSupplier/${id}`, {}, configuration);
      dispatch("fetchSuppliers");
    } catch (error) {
      commit("SET_ERROR", error.response);
      throw error;
    }
  },
};

const getters = {
  suppliers: (state) => state.suppliers,
  selectedSupplier: (state) => state.selectedSupplier,
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
