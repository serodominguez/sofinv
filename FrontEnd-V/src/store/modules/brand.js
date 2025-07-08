import axios from "axios";
import { jwtDecode } from "jwt-decode";
import store from "@/store";

const state = {
  brands: [],
  selectedBrand: null,
  loading: false,
  error: null,
};

const mutations = {
  SET_BRANDS(state, brands) {
    state.brands = brands;
  },
  SET_SELECTED_BRAND(state, brand) {
    state.selectedBrand = brand;
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
  async fetchBrands({ commit, rootState }) {
    commit("SET_LOADING", true);
    try {
      const token = rootState.token;
      if (isExpired(token)) {
        await store.dispatch("logoff");
        return;
      }

      const header = { Authorization: `Bearer ${token}` };
      let configuration = { headers: header };
      const response = await axios.get("api/Brands/ReadBrands", configuration);
      commit("SET_BRANDS", response.data);
    } catch (error) {
      commit("SET_ERROR", error.response ? error.response.data.message : error.message);
      throw error;
    } finally {
      commit("SET_LOADING", false);
    }
  },
  async fetchBrand({ commit, rootState }, text) {
    commit("SET_LOADING", true);
    try {
      const token = rootState.token;
      if (isExpired(token)) {
        await store.dispatch("logoff");
        return;
      }

      let header = { Authorization: "Bearer " + token };
      let configuration = { headers: header };
      const response = await axios.get(`api/Brands/SearchBrand/${text}`, configuration);
      commit("SET_BRANDS", response.data);
    } catch (error) {
      commit("SET_ERROR", error.message);
    } finally {
      commit("SET_LOADING", false);
    }
  },
  async selectBrands({ commit, rootState }) {
    commit("SET_LOADING", true);
    try {
      const token = rootState.token;
      if (isExpired(token)) {
        await store.dispatch("logoff");
        return;
      }

      let header = { Authorization: "Bearer " + token };
      let configuration = { headers: header };
      const response = await axios.get("api/Brands/SelectBrands", configuration);
      commit("SET_BRANDS", response.data);
    } catch (error) {
      commit("SET_ERROR", error.message);
    } finally {
      commit("SET_LOADING", false);
    }
  },
  async createBrand({ commit, dispatch, rootState }, brand) {
    try {
      const token = rootState.token;
      if (isExpired(token)) {
        await store.dispatch("logoff");
        return;
      }

      let header = { Authorization: "Bearer " + token };
      let configuration = { headers: header };
      await axios.post("api/Brands/CreateBrand", brand, configuration);
      //dispatch("fetchBrands");
    } catch (error) {
      commit("SET_ERROR", error.response);
      throw error;
    }
  },
  async updateBrand({ commit, dispatch, rootState }, brand) {
    try {
      const token = rootState.token;
      if (isExpired(token)) {
        await store.dispatch("logoff");
        return;
      }

      let header = { Authorization: "Bearer " + token };
      let configuration = { headers: header };
      await axios.put("api/Brands/UpdateBrand", brand, configuration);
      //dispatch("fetchBrands");
    } catch (error) {
      commit("SET_ERROR", error.response);
      throw error;
    }
  },
  async enabledBrand({ commit, dispatch, rootState }, id) {
    try {
      const token = rootState.token;
      if (isExpired(token)) {
        await store.dispatch("logoff");
        return;
      }

      let header = { Authorization: "Bearer " + token };
      let configuration = { headers: header };
      await axios.put(`api/Brands/EnabledBrand/${id}`, {}, configuration);
      dispatch("fetchBrands");
    } catch (error) {
      commit("SET_ERROR", error.response);
      throw error;
    }
  },
  async disabledBrand({ commit, dispatch, rootState }, id) {
    try {
      const token = rootState.token;
      if (isExpired(token)) {
        await store.dispatch("logoff");
        return;
      }

      let header = { Authorization: "Bearer " + token };
      let configuration = { headers: header };
      await axios.put(`api/Brands/DisabledBrand/${id}`, {}, configuration);
      dispatch("fetchBrands");
    } catch (error) {
      commit("SET_ERROR", error.response);
      throw error;
    }
  },
};

const getters = {
  brands: (state) => state.brands,
  selectedBrand: (state) => state.selectedBrand,
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

/* State:
brands: Un array que contiene todas las marcas obtenidas de la API.
selectedBrand: Un objeto que representa la marca actualmente seleccionada para edición.
loading: Un booleano que indica si se está cargando información (por ejemplo, al hacer una solicitud a la API).
error: Un string que almacena cualquier mensaje de error que pueda ocurrir durante las operaciones.

Mutations:
SET_BRANDS(state, brands): Actualiza el estado de brands con la lista de marcas obtenidas.
SET_SELECTED_BRAND(state, brand): Establece la marca seleccionada en el estado.
SET_LOADING(state, loading): Actualiza el estado de loading para indicar si se está realizando una operación de carga.
SET_ERROR(state, error): Establece un mensaje de error en el estado.

Actions:
fetchBrands({ commit }): Realiza una solicitud a la API para obtener todas las marcas. Utiliza commit para llamar a las mutaciones correspondientes para actualizar el estado.
createBrand({ dispatch }, brand): Envía una solicitud a la API para crear una nueva marca y luego vuelve a obtener la lista de marcas.
updateBrand({ dispatch }, brand): Envía una solicitud a la API para actualizar una marca existente y luego vuelve a obtener la lista de marcas.
deleteBrand({ dispatch }, id): Envía una solicitud a la API para eliminar una marca y luego vuelve a obtener la lista de marcas.

Getters:
brands: Devuelve la lista de marcas del estado.
selectedBrand: Devuelve la marca seleccionada del estado.
loading: Devuelve el estado de carga.
error: Devuelve cualquier mensaje de error.

Export:
namespaced: true: Esto permite que el módulo sea utilizado de manera independiente, lo que significa que puedes acceder a sus propiedades y métodos utilizando el nombre del módulo (por ejemplo, store.modules.brand.fetchBrands).
Se exporta el objeto que contiene state, mutations, actions y getters. */
