import axios from 'axios';
import { jwtDecode } from "jwt-decode";
import store from "@/store";

const state = {
  receipts: [],
  detailsReceipt: [],
  selectedReceipt: null,
  loading: false,
  error: null,
};

const mutations = {
  SET_RECEIPTS(state, receipts) {
    state.receipts = receipts;
  },
  SET_DETAILS_RECEIPT(state, detailsReceipt) {
    state.detailsReceipt = detailsReceipt;
  },
  SET_SELECTED_RECEIPT(state, receipt) {
    state.selectedReceipt = receipt;
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
  async fetchReceipts({ commit, rootState }, id) {
    commit("SET_LOADING", true);
    try {
      const token = rootState.token;
      if (isExpired(token)) {
        await store.dispatch("logoff");
        return;
      }

      let header = { Authorization: "Bearer " + token };
      let configuration = { headers: header };
      const response = await axios.get(`api/GoodsReceipt/ReadReceipts/${id}`, configuration);
      commit("SET_RECEIPTS", response.data);
    } catch (error) {
      commit("SET_ERROR", error.message);
    } finally {
      commit("SET_LOADING", false);
    }
  },
  async fetchReceipt({ commit, rootState }, { text, id }) {
    commit("SET_LOADING", true);
    try {
      const token = rootState.token;
      if (isExpired(token)) {
        await store.dispatch("logoff");
        return;
      }

      let header = { Authorization: "Bearer " + token };
      let configuration = {
        headers: header,
        params: {
          text: text,
          id: id,
        },
      };

      const response = await axios.get("api/GoodsReceipt/SearchReceipt", configuration);
      commit("SET_RECEIPTS", response.data);
    } catch (error) {
      commit("SET_ERROR", error.message);
    } finally {
      commit("SET_LOADING", false);
    }
  },
  async fetchDetail({ commit, rootState }, id) {
    commit("SET_LOADING", true);
    try {
      const token = rootState.token;
      if (isExpired(token)) {
        await store.dispatch("logoff");
        return;
      }

      let header = { Authorization: "Bearer " + token };
      let configuration = { headers: header };
      const response = await axios.get(`api/GoodsReceipt/ReadDetails/${id}`, configuration);
      commit("SET_DETAILS_RECEIPT", response.data);
    } catch (error) {
      commit("SET_ERROR", error.message);
    } finally {
      commit("SET_LOADING", false);
    }
  },
  async createReceipt({ commit, rootState }, receipt) {
    try {
      const token = rootState.token;
      if (isExpired(token)) {
        await store.dispatch("logoff");
        return;
      }

      let header = { Authorization: "Bearer " + token };
      let configuration = { headers: header };
      await axios.post("api/GoodsReceipt/CreateReceipt", receipt, configuration);
    } catch (error) {
      commit("SET_ERROR", error.response);
      throw error;
    }
  },
  async disabledReceipt({ commit, rootState }, id) {
    try {
      const token = rootState.token;
      if (isExpired(token)) {
        await store.dispatch("logoff");
        return;
      }

      let header = { Authorization: "Bearer " + token };
      let configuration = { headers: header };
      await axios.put(`api/GoodsReceipt/DisabledReceipt/${id}`,{},configuration);
    } catch (error) {
      commit("SET_ERROR", error.response);
      throw error;
    }
  },
};

const getters = {
  receipts: (state) => state.receipts,
  selectedReceipt: (state) => state.selectedReceipt,
  detailsReceipt: (state) => state.detailsReceipt,
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
