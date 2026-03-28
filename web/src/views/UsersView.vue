<template>
  <div class="p-4 container-fluid">

    <!-- Search bar -->
    <div class="row g-4 mb-3">
      <div class="col-md-3">
        <a-input
          v-model:value="searchParams.name"
          placeholder="Nhập tên"
        />
      </div>
      <div class="col-md-3">
        <a-input
          v-model:value="searchParams.email"
          placeholder="Nhập email"
        />
      </div>
      <!-- <div class="col-md-2 d-flex justify-content-center">
        <a-select class="w-100" v-model:value="searchParams.s" placeholder="Chọn trạng thái">
          <a-option value="Hoạt động">Hoạt động</a-option>
          <a-option value="Ngừng hoạt động">Ngừng hoạt động</a-option>
          <a-option value="Khoá">Khoá</a-option>
        </a-select>
      </div>
      <div class="col-md-2 d-flex justify-content-center">
        <a-date-picker class="w-100" v-model:value="filter.startDate" placeholder="Từ ngày" />
      </div>
      <div class="col-md-2 d-flex justify-content-center">
        <a-date-picker class="w-100" v-model:value="filter.endDate" placeholder="Đến ngày" />
      </div> -->
      <div class="col-md-3 d-flex justify-content-around gap-4">
        <a-button class="w-50" type="primary" @click="search">Tìm kiếm</a-button>
        <a-button class="w-50" @click="resetFilter">Đặt lại</a-button>
      </div>
    </div>

    <!-- Action -->
    <div class="d-flex mb-3">
      <a-button type="primary" @click="showAddUserModal">Thêm tài khoản user</a-button>
    </div>

    <!-- Table users -->
    <a-table
      :columns="columns"
      :data-source="users"
      :pagination="paginationConfig"
      :bordered="true"
      @change="handleChangePageSize"
    >
      <template #bodyCell="{ column, record }">
        <template v-if="column.key === 'name'">
          <div class="d-flex align-items-center gap-2">
            <a-avatar 
              :src="record.avatarUrl" 
              :size="32"
              :style="{ backgroundColor: '#1890ff' }"
            >
              {{ record.name?.charAt(0).toUpperCase() || 'U' }}
            </a-avatar>
            <span>{{ record.name }}</span>
          </div>
        </template>

        <template v-if="column.key === 'level'">
          <span>{{ `Lv.${record.level}` }}</span>
        </template>

        <template v-if="column.key === 'status'">
          <a-tag :color="
                record.status === 'Active' ? 'green' :
                record.status === 'Banned' ? 'red' :
                record.status === 'Inactive' ? 'processing' :
                record.status === 'Deleted' ? 'default' :
                'default'
            ">{{ record.status }}</a-tag>
        </template>

        <template v-if="column.key === 'action'">
          <div class="d-flex text-primary gap-4 fs-5 justify-content-center">
            <edit-outlined class="cursor-pointer" @click="showEditUserModal(record)" />
            <delete-outlined class="cursor-pointer" @click="showDeleteConfirm(record)" />
          </div>
        </template>
      </template>
    </a-table>

    <!-- Add/Edit User Modal -->
    <a-modal
      v-model:open="isUserModalOpen"
      :title="isEditMode ? 'Cập nhật User' : 'Thêm User'"
      @ok="handleUserFormSubmit"
      ok-text="Lưu"
      cancel-text="Hủy"
      :confirm-loading="userFormLoading"
      width="600px"
    >
      <a-form layout="vertical">
        <a-form-item v-if="isEditMode" label="Ảnh đại diện">
          <div class="mb-3">
            <a-avatar v-if="userFormData.avatarPreview" :src="userFormData.avatarPreview" :size="80" />
            <a-avatar v-else :src="userFormData.avatarUrl" :size="80" />
          </div>
          <input
            ref="avatarInput"
            type="file"
            accept="image/*"
            style="display: none"
            @change="handleAvatarChange"
          />
          <a-button @click="triggerAvatarInput">Chọn ảnh</a-button>
        </a-form-item>

        <a-form-item label="Tên">
          <a-input
            v-model:value="userFormData.name"
            placeholder="Nhập tên người dùng"
          />
        </a-form-item>

        <a-form-item label="Email">
          <a-input
            v-model:value="userFormData.email"
            placeholder="Nhập email"
            type="email"
            :disabled="isEditMode"
          />
        </a-form-item>

        <a-form-item label="Số điện thoại">
          <a-input
            v-model:value="userFormData.phoneNumber"
            placeholder="Nhập số điện thoại"
          />
        </a-form-item>

        <a-form-item v-if="!isEditMode" label="Mật khẩu">
          <a-input-password
            v-model:value="userFormData.password"
            placeholder="Nhập mật khẩu"
          />
        </a-form-item>

        <a-form-item label="Quyền">
          <a-select
            v-model:value="userFormData.roleId"
            placeholder="Chọn quyền"
          >
            <a-select-option :value="1">Super Admin</a-select-option>
            <a-select-option :value="2">Admin</a-select-option>
            <a-select-option :value="3">Moderator</a-select-option>
            <a-select-option :value="4">Editor</a-select-option>
            <a-select-option :value="5">User</a-select-option>
          </a-select>
        </a-form-item>

        <a-form-item v-if="isEditMode" label="Trạng thái">
          <a-select
            v-model:value="userFormData.status"
            placeholder="Chọn trạng thái"
          >
            <a-select-option :value="0">Active</a-select-option>
            <a-select-option :value="1">Inactive</a-select-option>
            <a-select-option :value="2">Banned</a-select-option>
            <a-select-option :value="3">Deleted</a-select-option>
          </a-select>
        </a-form-item>

        <a-form-item v-if="isEditMode" label="Cấp độ">
          <a-input-number
            v-model:value="userFormData.level"
            placeholder="Nhập cấp độ"
            :min="0"
          />
        </a-form-item>

        <a-form-item v-if="isEditMode" label="Hạng">
          <a-input-number
            v-model:value="userFormData.rank"
            placeholder="Nhập hạng"
            :min="0"
          />
        </a-form-item>

        <a-form-item v-if="isEditMode" label="Điểm hạng">
          <a-input-number
            v-model:value="userFormData.rankScore"
            placeholder="Nhập điểm hạng"
            :min="0"
          />
        </a-form-item>

        <a-form-item v-if="isEditMode" label="Kinh nghiệm">
          <a-input-number
            v-model:value="userFormData.exp"
            placeholder="Nhập kinh nghiệm"
            :min="0"
          />
        </a-form-item>
      </a-form>
    </a-modal>
  </div>
</template>

<script setup lang="ts">
import { ref, reactive, onMounted } from 'vue';
import { EditOutlined, DeleteOutlined } from '@ant-design/icons-vue';
import { SearchUserRequest, UserListItemDto, CreateUserRequest, UpdateUserRequest } from '@/types';
import { message, Modal } from 'ant-design-vue';
import { getAllUsers, createUser, updateUser, deleteUser, getUserDetail } from '@/api';
import { UserDetailDto } from '@/types/user';

const columns = [
  { title: 'ID', dataIndex: 'id', key: 'id', width: 80 },
  { title: 'Tên hiển thị', dataIndex: 'name', key: 'name' },
  { title: 'Email', dataIndex: 'email', key: 'email' },
  { title: 'Trạng thái', dataIndex: 'status', key: 'status' },
  { title: 'Quyền', dataIndex: 'roleName', key: 'roleName' },
  { title: 'Hành động', key: 'action', align: 'center' }
];

const paginationConfig = reactive({
  current: 1,
  pageSize: 5,
  showSizeChanger: true,
  pageSizeOptions: ['5', '10', '20', '50'],
  total: 0,
});

const resetFilter = () => {
  searchParams.email = '';
  searchParams.name = '';
  paginationConfig.current = 1;
  search();
}

const handleChangePageSize = (pagination: any): void => {
  paginationConfig.current = pagination.current;
  paginationConfig.pageSize = pagination.pageSize;
  search();
}

const users = ref<UserListItemDto[]>([]);
const searchParams = reactive<SearchUserRequest>({
  name: "",
  isAsc: true,
  pageIndex: 1,
  pageSize: 5,
  email: "",
  orderBy: "Id",
  phone: ""
});

// Modal states
const isUserModalOpen = ref(false);
const isEditMode = ref(false);
const userFormLoading = ref(false);
const currentEditingUserId = ref<number | null>(null);

// Form data
const userFormData = reactive<any>({
  name: '',
  email: '',
  phoneNumber: '',
  password: '',
  roleId: 1,
  status: 0,
  level: 0,
  rank: 0,
  rankScore: 0,
  exp: 0,
  avatar: null,
  avatarUrl: '',
  avatarPreview: ''
});

const avatarInput = ref<HTMLInputElement | null>(null);

const resetUserForm = () => {
  userFormData.name = '';
  userFormData.email = '';
  userFormData.phoneNumber = '';
  userFormData.password = '';
  userFormData.roleId = 1;
  userFormData.status = 0;
  userFormData.level = 0;
  userFormData.rank = 0;
  userFormData.rankScore = 0;
  userFormData.exp = 0;
  userFormData.avatar = null;
  userFormData.avatarUrl = '';
  userFormData.avatarPreview = '';
  currentEditingUserId.value = null;
};

const showAddUserModal = () => {
  isEditMode.value = false;
  resetUserForm();
  isUserModalOpen.value = true;
};

const showEditUserModal = async (user: UserListItemDto) => {
  isEditMode.value = true;
  userFormLoading.value = true;
  
  try {
    const apiRes = await getUserDetail(user.id);
    if (apiRes.isSuccess && apiRes.data) {
      const detail = apiRes.data as UserDetailDto;
      userFormData.name = detail.name;
      userFormData.email = detail.email;
      userFormData.phoneNumber = detail.phoneNumber;
      userFormData.roleId = detail.roleId;
      userFormData.status = detail.status;
      userFormData.level = detail.level;
      userFormData.rank = detail.rank;
      userFormData.rankScore = detail.rankScore;
      userFormData.exp = detail.exp;
      userFormData.avatarUrl = detail.avatarUrl || '';
      userFormData.avatarPreview = null;
      userFormData.avatar = null;
      currentEditingUserId.value = detail.id;
    } else {
      message.error(apiRes.message);
    }
  } catch (error) {
    message.error('Lỗi khi tải dữ liệu người dùng');
  } finally {
    userFormLoading.value = false;
  }
  
  isUserModalOpen.value = true;
};

const handleUserFormSubmit = async () => {
  if (!userFormData.name || !userFormData.email || !userFormData.phoneNumber || !userFormData.roleId) {
    message.error('Vui lòng điền đầy đủ các trường bắt buộc');
    return;
  }

  if (!isEditMode.value && !userFormData.password) {
    message.error('Vui lòng nhập mật khẩu');
    return;
  }

  userFormLoading.value = true;
  
  try {
    if (isEditMode.value && currentEditingUserId.value) {
      const updateRequest: any = {
        name: userFormData.name,
        email: userFormData.email,
        phoneNumber: userFormData.phoneNumber,
        roleId: userFormData.roleId,
        status: userFormData.status,
        level: userFormData.level,
        rank: userFormData.rank,
        rankScore: userFormData.rankScore,
        exp: userFormData.exp
      };

      // Thêm avatar nếu có
      if (userFormData.avatar) {
        updateRequest.avatar = userFormData.avatar;
      }
      
      const apiRes = await updateUser(currentEditingUserId.value, updateRequest);
      if (apiRes.isSuccess) {
        message.success('Cập nhật người dùng thành công');
        isUserModalOpen.value = false;
        search();
      } else {
        message.error(apiRes.message);
      }
    } else {
      const createRequest: CreateUserRequest = {
        name: userFormData.name,
        email: userFormData.email,
        phoneNumber: userFormData.phoneNumber,
        password: userFormData.password,
        roleId: userFormData.roleId
      };
      
      const apiRes = await createUser(createRequest);
      if (apiRes.isSuccess) {
        message.success('Thêm người dùng thành công');
        isUserModalOpen.value = false;
        search();
      } else {
        message.error(apiRes.message);
      }
    }
  } catch (error) {
    message.error('Lỗi khi xử lý yêu cầu');
  } finally {
    userFormLoading.value = false;
  }
};

const showDeleteConfirm = (user: UserListItemDto) => {
  Modal.confirm({
    title: 'Xác nhận xóa',
    content: `Bạn có chắc chắn muốn xóa người dùng ${user.name}?`,
    okText: 'Xóa',
    okType: 'danger',
    cancelText: 'Hủy',
    onOk() {
      handleDeleteUser(user.id);
    },
    onCancel() {
      // Cancel action
    },
  });
};

const handleDeleteUser = async (userId: number) => {
  try {
    const apiRes = await deleteUser(userId);
    if (apiRes.isSuccess) {
      message.success('Xóa người dùng thành công');
      search();
    } else {
      message.error(apiRes.message);
    }
  } catch (error) {
    message.error('Lỗi khi xóa người dùng');
  }
};

const triggerAvatarInput = () => {
  if (avatarInput.value) {
    avatarInput.value.click();
  }
};

const handleAvatarChange = (event: Event) => {
  const target = event.target as HTMLInputElement;
  const files = target.files;
  
  if (files && files.length > 0) {
    const file = files[0];
    userFormData.avatar = file;
    
    // Tạo preview
    const reader = new FileReader();
    reader.onload = (e: ProgressEvent<FileReader>) => {
      userFormData.avatarPreview = e.target?.result as string;
    };
    reader.readAsDataURL(file);
  }
};

const search = async () => {
  const hideLoading = message.loading("Đang tải dữ liệu...", 0);
  searchParams.pageIndex = paginationConfig.current;
  searchParams.pageSize = paginationConfig.pageSize;
  const apiRes = await getAllUsers(searchParams);
  hideLoading();

  if (apiRes.isSuccess) {
    users.value = apiRes.data!.items;
    paginationConfig.current = apiRes.data!.pageIndex;
    paginationConfig.pageSize = apiRes.data!.pageSize;
    paginationConfig.total = apiRes.data!.totalItems;
  }
};

onMounted(() => {
  search();
});


</script>

<style scoped>
.cursor-pointer {
  cursor: pointer;
  transition: opacity 0.2s;
}

.cursor-pointer:hover {
  opacity: 0.7;
}

:deep(.ant-table-thead > tr > th) {
  background-color: #2f56ed;
  color: #fff;
}
</style>
