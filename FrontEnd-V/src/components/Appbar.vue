<template>
  <nav>
    <v-app-bar color="blue-darken-4" dark app>
      <v-app-bar-nav-icon v-if="loggedin" @click.stop="drawer = !drawer"></v-app-bar-nav-icon>
      <v-toolbar-title v-if="loggedin" class="text-uppercase">
        <span class="font-weight-light"></span>
        <span style="font-size: 70%"><strong>Sucursal: {{ this.$store.state.currentUser .store_name }}</strong></span>
      </v-toolbar-title>
      <v-spacer></v-spacer>
      <span v-if="loggedin" style="font-size: 90%; margin-right: 10px;"><strong> Usuario: {{
          this.$store.state.currentUser .user_name }}</strong></span>
      <v-btn v-if="loggedin" @click="logoff" icon="logout"></v-btn>
    </v-app-bar>
  </nav>
  <v-navigation-drawer v-model="drawer" fixed app temporary v-if="loggedin">
    <v-list>
      <v-list-item variant="plain" :to="{ name: 'about' }">
        <v-list-item prepend-icon="home" title="Home"></v-list-item>
      </v-list-item>
      <v-list-group>
        <template v-if="isAdministrator || isGrocer" v-slot:activator="{ props }">
          <v-list-item v-bind="props" title="Almacén"></v-list-item>
        </template>
        <v-list-item rounded="xl" class="ma-0 ml-n10" v-for="link in linkStore" :key="link.text" :to="link.route">
          <template v-slot:prepend>
            <v-icon :icon="link.icon" style="font-size: 20px;"></v-icon>
          </template>
          <v-list-item-title v-text="link.text" style="font-size: 15px;"></v-list-item-title>
        </v-list-item>
        <v-list-group class="ma-0 ml-n15">
          <template v-slot:activator="{ props }">
            <v-list-item v-bind="props" title="Traspasos"></v-list-item>
          </template>
          <v-list-item rounded="xl" class="ma-0 ml-n10" v-for="link in linkTransfers" :key="link.text" :to="link.route">
            <template v-slot:prepend>
            <v-icon :icon="link.icon" style="font-size: 20px;"></v-icon>
          </template>
          <v-list-item-title v-text="link.text" style="font-size: 15px;"></v-list-item-title>
          </v-list-item>
        </v-list-group>
      </v-list-group>
      <v-list-group>
        <template v-if="isAdministrator" v-slot:activator="{ props }">
          <v-list-item v-bind="props" title="Accesos"></v-list-item>
        </template>
        <v-list-item rounded="xl" class="ma-0 ml-n10" v-for="link in linkAccess" :key="link.text" :to="link.route">
          <template v-slot:prepend>
            <v-icon :icon="link.icon" style="font-size: 20px;"></v-icon>
          </template>
          <v-list-item-title v-text="link.text" style="font-size: 15px;"></v-list-item-title>
        </v-list-item>
      </v-list-group>
    </v-list>
  </v-navigation-drawer>
</template>
<script>
export default {
  name: "Appbar",
  data: () => ({
    drawer: false,
    linkStore: [
      { icon: 'add_shopping_cart', text: 'Entradas', route: '/receipts' },
      { icon: 'category', text: 'Categorías', route: '/categories' },
      { icon: 'account_box', text: 'Clientes', route: '/customers' },
      { icon: 'copyright', text: 'Marcas', route: '/brands' },
      { icon: 'inventory_2', text: 'Productos', route: '/products' },
      { icon: 'contacts', text: 'Proveedores', route: '/suppliers' },
      { icon: 'remove_shopping_cart', text: 'Salidas', route: '/issues' },
      { icon: 'home_work', text: 'Sucursales', route: '/stores' }
    ],
    linkTransfers: [
      { icon: 'move_up', text: 'Entrada', route: '' },
      { icon: 'move_down', text: 'Salida', route: '' },
    ],
    linkAccess: [
      { icon: 'manage_accounts', text: 'Roles', route: '/roles' },
      { icon: 'person', text: 'Usuarios', route: '/users' },
    ],
  }),
  computed: {
    loggedin() {
      return this.$store.state.currentUser ;
    },
    isAdministrator() {
      return (
        this.$store.state.currentUser  &&
        this.$store.state.currentUser .role == "ADMINISTRADORES"
      );
    },
    isGrocer() {
      return (
        this.$store.state.currentUser  &&
        this.$store.state.currentUser .role == "ALMACENEROS"
      );
    },
    isOperator() {
      return (
        this.$store.state.currentUser  &&
        this.$store.state.currentUser .role == "OPERARIOS"
      );
    }
  },
  created() {
    this.$store.dispatch("auto");
  },
  methods: {
    logoff() {
      this.$store.dispatch("logoff");
    },
  },
}
</script>