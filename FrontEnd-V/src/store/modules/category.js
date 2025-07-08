import axios from "axios";
import { jwtDecode } from "jwt-decode";
import store from "@/store";

const state = {
  categories: [],
  selectedCategory: null,
  loading: false,
  error: null,
};

const mutations = {
  SET_CATEGORIES(state, categories) {
    state.categories = categories;
  },
  SET_SELECTED_CATEGORY(state, category) {
    state.selectedCategory = category;
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
  async fetchCategories({ commit, rootState }) {
    commit("SET_LOADING", true);
    try {
      const token = rootState.token;
      if (isExpired(token)) {
        await store.dispatch("logoff");
        return;
      }

      let header = { Authorization: "Bearer " + token };
      let configuration = { headers: header };
      const response = await axios.get("api/Categories/ReadCategories", configuration);
      commit("SET_CATEGORIES", response.data);
    } catch (error) {
      commit("SET_ERROR", error.message);
    } finally {
      commit("SET_LOADING", false);
    }
  },
  async fetchCategory({ commit, rootState }, text) {
    commit("SET_LOADING", true);
    try {
      const token = rootState.token;
      if (isExpired(token)) {
        await store.dispatch("logoff");
        return;
      }

      let header = { Authorization: "Bearer " + token };
      let configuration = { headers: header };
      const response = await axios.get(`api/Categories/SearchCategory/${text}`, configuration);
      commit("SET_CATEGORIES", response.data);
    } catch (error) {
      commit("SET_ERROR", error.message);
    } finally {
      commit("SET_LOADING", false);
    }
  },
  async selectCategories({ commit, rootState }) {
    commit("SET_LOADING", true);
    try {
      const token = rootState.token;
      if (isExpired(token)) {
        await store.dispatch("logoff");
        return;
      }

      let header = { Authorization: "Bearer " + token };
      let configuration = { headers: header };
      const response = await axios.get("api/Categories/SelectCategories", configuration);
      commit("SET_CATEGORIES", response.data);
    } catch (error) {
      commit("SET_ERROR", error.message);
    } finally {
      commit("SET_LOADING", false);
    }
  },
  async createCategory({ commit, rootState }, category) {
    try {
      const token = rootState.token;
      if (isExpired(token)) {
        await store.dispatch("logoff");
        return;
      }

      let header = { Authorization: "Bearer " + token };
      let configuration = { headers: header };
      await axios.post("api/Categories/CreateCategory", category, configuration);
    } catch (error) {
      commit("SET_ERROR", error.response);
      throw error;
    }
  },
  async updateCategory({ commit, rootState }, category) {
    try {
      const token = rootState.token;
      if (isExpired(token)) {
        await store.dispatch("logoff");
        return;
      }

      let header = { Authorization: "Bearer " + token };
      let configuration = { headers: header };
      await axios.put("api/Categories/UpdateCategory", category, configuration);
    } catch (error) {
      commit("SET_ERROR", error.response);
      throw error;
    }
  },
  async enabledCategory({ commit, dispatch, rootState }, id) {
    try {
      const token = rootState.token;
      if (isExpired(token)) {
        await store.dispatch("logoff");
        return;
      }

      let header = { Authorization: "Bearer " + token };
      let configuration = { headers: header };
      await axios.put(`api/Categories/EnabledCategory/${id}`, {}, configuration);
      dispatch("fetchCategories");
    } catch (error) {
      commit("SET_ERROR", error.response);
      throw error;
    }
  },
  async disabledCategory({ commit, dispatch, rootState }, id) {
    try {
      const token = rootState.token;
      if (isExpired(token)) {
        await store.dispatch("logoff");
        return;
      }

      let header = { Authorization: "Bearer " + token };
      let configuration = { headers: header };
      await axios.put(`api/Categories/DisabledCategory/${id}`, {}, configuration);
      dispatch("fetchCategories");
    } catch (error) {
      commit("SET_ERROR", error.response);
      throw error;
    }
  },
};

const getters = {
  categories: (state) => state.categories,
  selectedCategory: (state) => state.selectedCategory,
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
