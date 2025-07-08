import { createStore } from 'vuex';
import { jwtDecode } from 'jwt-decode';
import router from '@/router/index'
import brand from './modules/brand';
import category from './modules/category';
import customer from './modules/customer';
import goodsissue from './modules/goodsissue';
import goodsreceipt from './modules/goodsreceipt';
import product from './modules/product';
import role from './modules/role';
import store from './modules/store';
import supplier from './modules/supplier';
import user from './modules/user';
import warehouse from './modules/warehouse';

const storeJS = createStore({
  state: {
    token: null,
    currentUser: null,
  },

  getters: {
    isAuthenticated(state) {
      return !!state.token;
    },
    getCurrentUser (state) {
      return state.currentUser ;
    }
  },

  mutations: {
    SET_TOKEN(state, token) {
      state.token = token
    },
    SET_USER(state, user) {
      state.currentUser = user
    }
  },

  actions: {
    saveToken({ commit }, token) {
      commit("SET_TOKEN", token);
      commit("SET_USER", jwtDecode(token));
      localStorage.setItem("token", token);
    },
    auto({ commit, dispatch }) {
      let token = localStorage.getItem("token");
      if (token) {
        const decodedToken = jwtDecode(token);
        const currentTime = Date.now() / 1000;
        if (decodedToken.exp < currentTime) {
          dispatch('logoff');
          return;
        } else {
          commit("SET_TOKEN", token);
          commit("SET_USER", jwtDecode(token));
        }
      }
      router.push({ name: "dashboard" });
    },
    logoff({ commit }) {
      commit("SET_TOKEN", null);
      commit("SET_USER", null);
      localStorage.removeItem("token");
      router.push({ name: "login" });
    },
  },

  modules: {
    brand,
    category,
    customer,
    goodsissue,
    goodsreceipt,
    product,
    role,
    store,
    supplier,
    user,
    warehouse,
  },
});

export default storeJS;