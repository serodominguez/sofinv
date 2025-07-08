<template>
  <v-app>
    <Appbar/>
    <v-main class="ma-4">
      <router-view></router-view>
    </v-main>
    <Footerbar/>
  </v-app>
</template>

<script>
import Appbar from "./components/Appbar.vue";
import Footerbar from "./components/Footerbar.vue";
export default{
  name: 'App',
  components:{
    Appbar,
    Footerbar
  }
}
</script>
