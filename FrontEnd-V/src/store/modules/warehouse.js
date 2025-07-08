import axios from "axios";
import { jwtDecode } from "jwt-decode";
import store from "@/store";

const state = {
  warehouses: [],
  loading: false,
  error: null,
};

const mutations = {
  SET_WAREHOUSES(state, warehouses) {
    state.warehouses = warehouses;
  },
  SET_LOADING(state, loading) {
    state.loading = loading;
  },
  SET_ERROR(state, error) {
    state.error = error;
  },
  SET_CLEAR(state) {
    state.warehouses = [];
  },
};

const isExpired = (token) => {
  if (!token) return false;
  const decodedToken = jwtDecode(token);
  const currentTime = Date.now() / 1000;
  return decodedToken.exp < currentTime;
};

const actions = {
  async selectProducts({ commit, rootState }, { text, id }) {
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
      const response = await axios.get(`api/Warehouses/SelectProduct`, configuration);
      commit("SET_WAREHOUSES", response.data);
    } catch (error) {
      commit("SET_ERROR", error.message);
    } finally {
      commit("SET_LOADING", false);
    }
  },
};

const getters = {
  warehouses: (state) => state.warehouses,
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
