import axios from "axios";
import { jwtDecode } from "jwt-decode";
import store from "@/store";

const state = {
  users: [],
  selectedUser: null,
  loading: false,
  error: null,
};

const mutations = {
  SET_USERS(state, users) {
    state.users = users;
  },
  SET_SELECTED_USER(state, user) {
    state.selectedUser = user;
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
  async fetchUsers({ commit, rootState }) {
    commit("SET_LOADING", true);
    try {
      const token = rootState.token;
      if (isExpired(token)) {
        await store.dispatch("logoff");
        return;
      }

      let header = { Authorization: "Bearer " + token };
      let configuration = { headers: header };
      const response = await axios.get("api/Users/ReadUsers", configuration);
      commit("SET_USERS", response.data);
    } catch (error) {
      commit("SET_ERROR", error.message);
    } finally {
      commit("SET_LOADING", false);
    }
  },
  async fetchUser({ commit, rootState }, text) {
    commit("SET_LOADING", true);
    try {
      const token = rootState.token;
      if (isExpired(token)) {
        await store.dispatch("logoff");
        return;
      }

      let header = { Authorization: "Bearer " + token };
      let configuration = { headers: header };
      const response = await axios.get(`api/Users/SearchUser/${text}`, configuration);
      commit("SET_USERS", response.data);
    } catch (error) {
      commit("SET_ERROR", error.message);
    } finally {
      commit("SET_LOADING", false);
    }
  },
  async createUser({ commit, rootState }, userData) {
    try {
      const token = rootState.token;
      if (isExpired(token)) {
        await store.dispatch("logoff");
        return;
      }

      let header = { Authorization: "Bearer " + token };
      let configuration = { headers: header };
      await axios.post("api/Users/CreateUser", userData, configuration);
    } catch (error) {
      commit("SET_ERROR", error.response);
      throw error;
    }
  },
  async updateUser({ commit, rootState }, userData) {
    try {
      const token = rootState.token;
      if (isExpired(token)) {
        await store.dispatch("logoff");
        return;
      }

      let header = { Authorization: "Bearer " + token };
      let configuration = { headers: header };
      await axios.put("api/Users/UpdateUser", userData, configuration);
    } catch (error) {
      commit("SET_ERROR", error.response);
      throw error;
    }
  },
  async enabledUser({ commit, dispatch, rootState }, id) {
    try {
      const token = rootState.token;
      if (isExpired(token)) {
        await store.dispatch("logoff");
        return;
      }

      let header = { Authorization: "Bearer " + token };
      let configuration = { headers: header };
      await axios.put(`api/Users/EnabledUser/${id}`, {}, configuration);
      dispatch("fetchUsers");
    } catch (error) {
      commit("SET_ERROR", error.response);
      throw error;
    }
  },
  async disabledUser({ commit, dispatch, rootState }, id) {
    try {
      const token = rootState.token;
      if (isExpired(token)) {
        await store.dispatch("logoff");
        return;
      }

      let header = { Authorization: "Bearer " + token };
      let configuration = { headers: header };
      await axios.put(`api/Users/DisabledUser/${id}`, {}, configuration);
      dispatch("fetchUsers");
    } catch (error) {
      commit("SET_ERROR", error.response);
      throw error;
    }
  },
};

const getters = {
  users: (state) => state.users,
  selectedUser: (state) => state.selectedUser,
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
