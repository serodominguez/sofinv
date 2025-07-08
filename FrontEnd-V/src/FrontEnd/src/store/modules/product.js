import axios from "axios";
import { jwtDecode } from "jwt-decode";
import store from "@/store";

const state = {
  products: [],
  selectedProduct: null,
  loading: false,
  error: null,
};

const mutations = {
  SET_PRODUCTS(state, products) {
    state.products = products;
  },
  SET_SELECTED_PRODUCT(state, product) {
    state.selectedProduct = product;
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
  async fetchProducts({ commit, rootState }) {
    commit("SET_LOADING", true);
    try {
      const token = rootState.token;
      if (isExpired(token)) {
        await store.dispatch("logoff");
        return;
      }

      let header = { Authorization: "Bearer " + token };
      let configuration = { headers: header };
      const response = await axios.get("api/Products/ReadProducts", configuration);
      commit("SET_PRODUCTS", response.data);
    } catch (error) {
      commit("SET_ERROR", error.message);
    } finally {
      commit("SET_LOADING", false);
    }
  },
  async fetchProduct({ commit, rootState }, text) {
    commit("SET_LOADING", true);
    try {
      const token = rootState.token;
      if (isExpired(token)) {
        await store.dispatch("logoff");
        return;
      }

      let header = { Authorization: "Bearer " + token };
      let configuration = { headers: header };
      const response = await axios.get(`api/Products/SearchProduct/${text}`, configuration);
      commit("SET_PRODUCTS", response.data);
    } catch (error) {
      commit("SET_ERROR", error.message);
    } finally {
      commit("SET_LOADING", false);
    }
  },
  async createProduct({ commit, rootState }, product) {
    try {
      const token = rootState.token;
      if (isExpired(token)) {
        await store.dispatch("logoff");
        return;
      }

      let header = { Authorization: "Bearer " + token };
      let configuration = { headers: header };
      await axios.post("api/Products/CreateProduct", product, configuration);
    } catch (error) {
      commit("SET_ERROR", error.response);
      throw error;
    }
  },
  async updateProduct({ commit, rootState }, product) {
    try {
      const token = rootState.token;
      if (isExpired(token)) {
        await store.dispatch("logoff");
        return;
      }

      let header = { Authorization: "Bearer " + token };
      let configuration = { headers: header };
      await axios.put("api/Products/UpdateProduct", product, configuration);
    } catch (error) {
      commit("SET_ERROR", error.response);
      throw error;
    }
  },
  async enabledProduct({ commit, dispatch, rootState }, id) {
    try {
      const token = rootState.token;
      if (isExpired(token)) {
        await store.dispatch("logoff");
        return;
      }

      let header = { Authorization: "Bearer " + token };
      let configuration = { headers: header };
      await axios.put(`api/Products/EnabledProduct/${id}`, {}, configuration);
      dispatch("fetchProducts");
    } catch (error) {
      commit("SET_ERROR", error.response);
      throw error;
    }
  },
  async disabledProduct({ commit, dispatch, rootState }, id) {
    try {
      const token = rootState.token;
      if (isExpired(token)) {
        await store.dispatch("logoff");
        return;
      }

      let header = { Authorization: "Bearer " + token };
      let configuration = { headers: header };
      await axios.put(`api/Products/DisabledProduct/${id}`, {}, configuration);
      dispatch("fetchProducts");
    } catch (error) {
      commit("SET_ERROR", error.response);
      throw error;
    }
  },
};

const getters = {
  products: (state) => state.products,
  selectedProduct: (state) => state.selectedProduct,
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
