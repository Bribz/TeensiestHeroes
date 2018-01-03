using BeardedManStudios.Forge.Networking.Frame;
using BeardedManStudios.Forge.Networking.Unity;
using System;
using UnityEngine;

namespace BeardedManStudios.Forge.Networking.Generated
{
	[GeneratedInterpol("{\"inter\":[0.3,0.25]")]
	public partial class PlayerNetworkObject : NetworkObject
	{
		public const int IDENTITY = 5;

		private byte[] _dirtyFields = new byte[1];

		#pragma warning disable 0067
		public event FieldChangedEvent fieldAltered;
		#pragma warning restore 0067
		private Vector3 _m_Position;
		public event FieldEvent<Vector3> m_PositionChanged;
		public InterpolateVector3 m_PositionInterpolation = new InterpolateVector3() { LerpT = 0.3f, Enabled = true };
		public Vector3 m_Position
		{
			get { return _m_Position; }
			set
			{
				// Don't do anything if the value is the same
				if (_m_Position == value)
					return;

				// Mark the field as dirty for the network to transmit
				_dirtyFields[0] |= 0x1;
				_m_Position = value;
				hasDirtyFields = true;
			}
		}

		public void Setm_PositionDirty()
		{
			_dirtyFields[0] |= 0x1;
			hasDirtyFields = true;
		}

		private void RunChange_m_Position(ulong timestep)
		{
			if (m_PositionChanged != null) m_PositionChanged(_m_Position, timestep);
			if (fieldAltered != null) fieldAltered("m_Position", _m_Position, timestep);
		}
		private Quaternion _m_Rotation;
		public event FieldEvent<Quaternion> m_RotationChanged;
		public InterpolateQuaternion m_RotationInterpolation = new InterpolateQuaternion() { LerpT = 0.25f, Enabled = true };
		public Quaternion m_Rotation
		{
			get { return _m_Rotation; }
			set
			{
				// Don't do anything if the value is the same
				if (_m_Rotation == value)
					return;

				// Mark the field as dirty for the network to transmit
				_dirtyFields[0] |= 0x2;
				_m_Rotation = value;
				hasDirtyFields = true;
			}
		}

		public void Setm_RotationDirty()
		{
			_dirtyFields[0] |= 0x2;
			hasDirtyFields = true;
		}

		private void RunChange_m_Rotation(ulong timestep)
		{
			if (m_RotationChanged != null) m_RotationChanged(_m_Rotation, timestep);
			if (fieldAltered != null) fieldAltered("m_Rotation", _m_Rotation, timestep);
		}

		protected override void OwnershipChanged()
		{
			base.OwnershipChanged();
			SnapInterpolations();
		}
		
		public void SnapInterpolations()
		{
			m_PositionInterpolation.current = m_PositionInterpolation.target;
			m_RotationInterpolation.current = m_RotationInterpolation.target;
		}

		public override int UniqueIdentity { get { return IDENTITY; } }

		protected override BMSByte WritePayload(BMSByte data)
		{
			UnityObjectMapper.Instance.MapBytes(data, _m_Position);
			UnityObjectMapper.Instance.MapBytes(data, _m_Rotation);

			return data;
		}

		protected override void ReadPayload(BMSByte payload, ulong timestep)
		{
			_m_Position = UnityObjectMapper.Instance.Map<Vector3>(payload);
			m_PositionInterpolation.current = _m_Position;
			m_PositionInterpolation.target = _m_Position;
			RunChange_m_Position(timestep);
			_m_Rotation = UnityObjectMapper.Instance.Map<Quaternion>(payload);
			m_RotationInterpolation.current = _m_Rotation;
			m_RotationInterpolation.target = _m_Rotation;
			RunChange_m_Rotation(timestep);
		}

		protected override BMSByte SerializeDirtyFields()
		{
			dirtyFieldsData.Clear();
			dirtyFieldsData.Append(_dirtyFields);

			if ((0x1 & _dirtyFields[0]) != 0)
				UnityObjectMapper.Instance.MapBytes(dirtyFieldsData, _m_Position);
			if ((0x2 & _dirtyFields[0]) != 0)
				UnityObjectMapper.Instance.MapBytes(dirtyFieldsData, _m_Rotation);

			return dirtyFieldsData;
		}

		protected override void ReadDirtyFields(BMSByte data, ulong timestep)
		{
			if (readDirtyFlags == null)
				Initialize();

			Buffer.BlockCopy(data.byteArr, data.StartIndex(), readDirtyFlags, 0, readDirtyFlags.Length);
			data.MoveStartIndex(readDirtyFlags.Length);

			if ((0x1 & readDirtyFlags[0]) != 0)
			{
				if (m_PositionInterpolation.Enabled)
				{
					m_PositionInterpolation.target = UnityObjectMapper.Instance.Map<Vector3>(data);
					m_PositionInterpolation.Timestep = timestep;
				}
				else
				{
					_m_Position = UnityObjectMapper.Instance.Map<Vector3>(data);
					RunChange_m_Position(timestep);
				}
			}
			if ((0x2 & readDirtyFlags[0]) != 0)
			{
				if (m_RotationInterpolation.Enabled)
				{
					m_RotationInterpolation.target = UnityObjectMapper.Instance.Map<Quaternion>(data);
					m_RotationInterpolation.Timestep = timestep;
				}
				else
				{
					_m_Rotation = UnityObjectMapper.Instance.Map<Quaternion>(data);
					RunChange_m_Rotation(timestep);
				}
			}
		}

		public override void InterpolateUpdate()
		{
			if (IsOwner)
				return;

			if (m_PositionInterpolation.Enabled && !m_PositionInterpolation.current.Near(m_PositionInterpolation.target, 0.0015f))
			{
				_m_Position = (Vector3)m_PositionInterpolation.Interpolate();
				RunChange_m_Position(m_PositionInterpolation.Timestep);
			}
			if (m_RotationInterpolation.Enabled && !m_RotationInterpolation.current.Near(m_RotationInterpolation.target, 0.0015f))
			{
				_m_Rotation = (Quaternion)m_RotationInterpolation.Interpolate();
				RunChange_m_Rotation(m_RotationInterpolation.Timestep);
			}
		}

		private void Initialize()
		{
			if (readDirtyFlags == null)
				readDirtyFlags = new byte[1];

		}

		public PlayerNetworkObject() : base() { Initialize(); }
		public PlayerNetworkObject(NetWorker networker, INetworkBehavior networkBehavior = null, int createCode = 0, byte[] metadata = null) : base(networker, networkBehavior, createCode, metadata) { Initialize(); }
		public PlayerNetworkObject(NetWorker networker, uint serverId, FrameStream frame) : base(networker, serverId, frame) { Initialize(); }

		// DO NOT TOUCH, THIS GETS GENERATED PLEASE EXTEND THIS CLASS IF YOU WISH TO HAVE CUSTOM CODE ADDITIONS
	}
}