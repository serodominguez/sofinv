import axios from 'axios';
import { jwtDecode } from "jwt-decode";
import store from "@/store";

const state = {
  issues: [],
  detailsIssue: [],
  selectedIssue: null,
  loading: false,
  error: null,
};

const mutations = {
  SET_ISSUES(state, issues) {
    state.issues = issues;
  },
  SET_DETAILS_ISSUE(state, detailsIssue) {
    state.detailsIssue = detailsIssue;
  },
  SET_SELECTED_ISSUE(state, issue) {
    state.selectedIssue = issue;
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
  async fetchIssues({ commit, rootState }, id) {
    commit("SET_LOADING", true);
    try {
      const token = rootState.token;
      if (isExpired(token)) {
        await store.dispatch("logoff");
        return;
      }

      let header = { Authorization: "Bearer " + token };
      let configuration = { headers: header };
      const response = await axios.get(`api/GoodsIssue/ReadIssues/${id}`, configuration);
      commit("SET_ISSUES", response.data);
    } catch (error) {
      commit("SET_ERROR", error.message);
    } finally {
      commit("SET_LOADING", false);
    }
  },
  async fetchIssue({ commit, rootState }, { text, id }) {
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

      const response = await axios.get("api/GoodsIssue/SearchIssue", configuration);
      commit("SET_ISSUES", response.data);
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
      const response = await axios.get(`api/GoodsIssue/ReadDetails/${id}`, configuration);
      commit("SET_DETAILS_ISSUE", response.data);
    } catch (error) {
      commit("SET_ERROR", error.message);
    } finally {
      commit("SET_LOADING", false);
    }
  },
  async createIssue({ commit, rootState }, issue) {
    try {
      const token = rootState.token;
      if (isExpired(token)) {
        await store.dispatch("logoff");
        return;
      }

      let header = { Authorization: "Bearer " + token };
      let configuration = { headers: header };
      await axios.post("api/GoodsIssue/CreateIssue", issue, configuration);
    } catch (error) {
      commit("SET_ERROR", error.response);
      throw error;
    }
  },
  async disabledIssue({ commit, rootState }, id) {
    try {
      const token = rootState.token;
      if (isExpired(token)) {
        await store.dispatch("logoff");
        return;
      }

      let header = { Authorization: "Bearer " + token };
      let configuration = { headers: header };
      await axios.put(`api/GoodsIssue/DisabledIssue/${id}`,{},configuration);
    } catch (error) {
      commit("SET_ERROR", error.response);
      throw error;
    }
  },
};

const getters = {
  issues: (state) => state.issues,
  selectedIssue: (state) => state.selectedIssue,
  detailsIssue: (state) => state.detailsIssue,
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
